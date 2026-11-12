using System.Reflection;
using System.Data.Common;
using System.Runtime.CompilerServices;

using Microsoft.Data.SqlClient;

using Prism.Core.Metadata;
using Prism.Core.Attributes;
using Prism.Core.Connections;
using Prism.Core.MetadataBuilder;
using Prism.Core.EntityDiscovery;
using Prism.Core.SqlGeneration.DDL;
using Prism.Core.SqlGeneration.DML;

namespace Prism.Core.ORMContext
{
    /// <summary>
    /// The main entry point for interacting with the database through Prism.
    /// On construction, discovers all <c>PrismTableAttribute</c>-annotated entities in the entry assembly,
    /// builds their metadata, and runs a one-time migration to create any missing tables.
    /// Exposes typed CRUD methods that delegate SQL generation to the injected DDL/DML generators.
    /// Implements <see cref="IDisposable"/> - dispose the context when it is no longer needed to release the connection.
    /// </summary>
    public class PrismContext : IDisposable
    {
        private static bool _DbHasBeenInitialized;
        private readonly IDDLGenerator _DDLGenerator;
        private readonly IDMLGenerator _DMLGenerator;
        private readonly IEntitySearcher _EntitySearcher;
        private readonly IEntityMetadataBuilder _EntityMetadataBuilder;
        private readonly List<EntityMetadata> _Entities;
        private readonly PrismDbConnection _PrismDbConnection;
        private Dictionary<Type, string> _EntityDbMappings;

        /// <summary>
        /// Initialises the context, wires up default SQL Server implementations of all dependencies,
        /// discovers and builds metadata for every entity in the entry assembly, then runs the migration
        /// if it has not already been executed in this process.
        /// </summary>
        public PrismContext(string connectionString)
        {
            _DDLGenerator = new SQLServerDDLGenerator();
            _DMLGenerator = new SQLServerDMLGenerator();
            _EntitySearcher = new AttributeBasedEntityDiscovery();
            _EntityMetadataBuilder = new EntityMetadataBuilder();
            _PrismDbConnection = new PrismDbConnection(connectionString);
            _Entities = new List<EntityMetadata>();
            _EntityDbMappings = new Dictionary<Type, string>();

            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            foreach (Type type in _EntitySearcher.GetMappableTypes(assembly).ToList())
            {
                EntityMetadata entityMetadata = _EntityMetadataBuilder.BuildEntityMetadata(type);
                _Entities.Add(entityMetadata);
                _EntityDbMappings.Add(type, entityMetadata.TableName);
            }

            if (_DbHasBeenInitialized == false)
            {
                RunMigration();
            }
        }

        /// <summary>
        /// Fetches the entity of type <typeparamref name="T"/> with the given primary key,
        /// or returns <c>default</c> if no matching row exists.
        /// </summary>
        public T GetById<T>(int id)
        {
            SqlCommand sqlCommand = new SqlCommand(_DMLGenerator.GenerateGetByIdStatement(id, _EntityDbMappings[typeof(T)]), (SqlConnection)_PrismDbConnection.GetDbConnection());

            using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    return MapDataTableToEntity<T>(dataReader);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Deletes the entity of type <typeparamref name="T"/> with the given primary key.
        /// Returns <c>true</c> if a row was affected.
        /// </summary>
        public bool Delete<T>(int id)
        {
            Dictionary<string, object> values = new Dictionary<string, object> { { "Id", id } };
            SqlCommand sqlCommand = new SqlCommand(_DMLGenerator.GenerateDeleteStatement(values, _EntityDbMappings[typeof(T)]), (SqlConnection)_PrismDbConnection.GetDbConnection());
            int affectedRows = sqlCommand.ExecuteNonQuery();
            return affectedRows > 0;
        }

        /// <summary>
        /// Inserts <paramref name="entity"/> into its mapped table and returns <c>true</c> if a row was affected.
        /// </summary>
        public bool Insert<T>(T entity) where T : class
        {
            Dictionary<string, object> values = GetEntityValues<T>(entity);

            SqlCommand sqlCommand = new SqlCommand(_DMLGenerator.GenerateInsertStatement(values, _EntityDbMappings[typeof(T)]), (SqlConnection)_PrismDbConnection.GetDbConnection());
            int affectedRows = sqlCommand.ExecuteNonQuery();

            return affectedRows > 0;
        }

        private Dictionary<string, object> GetEntityValues<T>(T entity)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                PrismColumnAttribute? col = property.GetCustomAttribute<PrismColumnAttribute>();
                if (col != null)
                {
                    keyValuePairs.Add(col.Name, property.GetValue(entity));
                }
            }

            return keyValuePairs;
        }

        // All CREATE TABLE statements run inside a single transaction so a partial schema is never left behind.
        private void RunMigration()
        {
            List<SqlCommand> ddlCommands = new List<SqlCommand>();

            DbConnection dbConnection = _PrismDbConnection.GetDbConnection();

            // FK-dependent tables must be dropped first, then parent tables, so constraints don't block the drop.
            IEnumerable<EntityMetadata> dropOrder = _Entities
                .OrderBy(e => e.Columns.Any(c => c.ForeignKey != null) ? 0 : 1);

            // Parent tables must be created before FK-dependent tables.
            IEnumerable<EntityMetadata> createOrder = _Entities
                .OrderBy(e => e.Columns.Any(c => c.ForeignKey != null) ? 1 : 0);

            using (DbTransaction ddlTransaction = dbConnection.BeginTransaction())
            {
                foreach (EntityMetadata item in dropOrder)
                {
                    ddlCommands.Add(new SqlCommand($"DROP TABLE IF EXISTS {item.TableName}", (SqlConnection)dbConnection, (SqlTransaction)ddlTransaction));
                }

                foreach (EntityMetadata item in createOrder)
                {
                    ddlCommands.Add(new SqlCommand(_DDLGenerator.GenerateCreateTableStatement(item), (SqlConnection)dbConnection, (SqlTransaction)ddlTransaction));
                }

                try
                {
                    foreach (SqlCommand ddlCommand in ddlCommands)
                    {
                        ddlCommand.ExecuteNonQuery();
                    }

                    ddlTransaction.Commit();

                    _DbHasBeenInitialized = true;
                }
                catch (Exception)
                {
                    ddlTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Releases the underlying database connection. Call when the context is no longer needed.
        /// </summary>
        public void Dispose()
        {
            _PrismDbConnection.Dispose();
        }

        // Column names in the reader are matched by PrismColumnAttribute.Name, not the C# property name.
        private T MapDataTableToEntity<T>(SqlDataReader dataReader)
        {
            Type type = typeof(T);
            object entityClass = RuntimeHelpers.GetUninitializedObject(type);

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.GetCustomAttribute<PrismIgnoreAttribute>() != null) { continue; }

                PrismColumnAttribute? col = property.GetCustomAttribute<PrismColumnAttribute>();
                if (col != null)
                {
                    object raw = dataReader[col.Name];
                    object? value = raw == DBNull.Value
                        ? null
                        : Convert.ChangeType(raw, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                    property.SetValue(entityClass, value);
                }
            }

            return (T)entityClass;
        }
    }
}
