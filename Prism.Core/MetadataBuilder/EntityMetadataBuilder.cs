using System.Reflection;
using Prism.Core.Metadata;
using Prism.Core.Attributes;

namespace Prism.Core.MetadataBuilder
{
    /// <summary>
    /// Reflects over a Prism-annotated CLR type and constructs an <see cref="EntityMetadata"/> instance
    /// describing its table name, columns, primary key, and foreign key constraints.
    /// Called once per entity type during context initialisation; the resulting metadata is cached and
    /// reused for all subsequent SQL generation.
    /// </summary>
    public class EntityMetadataBuilder : IEntityMetadataBuilder
    {
        /// <summary>
        /// Entry point: builds the complete <see cref="EntityMetadata"/> for <paramref name="type"/>
        /// by delegating table-name and column resolution to private helpers.
        /// </summary>
        public EntityMetadata BuildEntityMetadata(Type type)
        {
            EntityMetadata entityMetadata = new EntityMetadata();
            entityMetadata.EntityType = type;
            entityMetadata.TableName = GetTableName(type);
            entityMetadata.Columns = GetColumns(type);

            return entityMetadata;
        }

        // Falls back to the class name when PrismTableAttribute is absent or Name is not set.
        private string GetTableName(Type type)
        {
            PrismTableAttribute? tableNameAttribute = type.GetCustomAttribute<PrismTableAttribute>();
            if (tableNameAttribute != null && !string.IsNullOrEmpty(tableNameAttribute.Name))
            {
                return tableNameAttribute.Name;
            }

            return type.Name;
        }

        // Iterates every public property; throws if a property lacks PrismColumnAttribute,
        // since unmapped properties would silently produce incomplete SQL.
        private List<ColumnMetadata> GetColumns(Type type)
        {
            List<PropertyInfo> propertyInfo = type.GetProperties().ToList();
            if (propertyInfo.Any() == false)
            {
                throw new Exception($"{type} contains no valid properties to map as columns.");
            }

            List<ColumnMetadata> columns = new List<ColumnMetadata>();

            foreach (PropertyInfo property in propertyInfo)
            {
                ColumnMetadata columnMetadata = new ColumnMetadata();

                PrismColumnAttribute? prismColumnAttribute = property.GetCustomAttribute<PrismColumnAttribute>();

                if (property.GetCustomAttribute<PrismIgnoreAttribute>() != null)
                {
                    continue;
                }

                if (prismColumnAttribute != null)
                {
                    columnMetadata.Name = prismColumnAttribute.Name;
                    columnMetadata.DataType = prismColumnAttribute.DataType;

                    // NullabilityInfoContext handles both Nullable<T> value types and nullable reference types (string?).
                    NullabilityInfoContext nullabilityContext = new NullabilityInfoContext();
                    NullabilityInfo nullabilityInfo = nullabilityContext.Create(property);
                    columnMetadata.IsNullable = nullabilityInfo.WriteState == NullabilityState.Nullable;

                    // Get if the column is a primary key.
                    PrismKeyAttribute? prismKeyAttribute = property.GetCustomAttribute<PrismKeyAttribute>();
                    if (prismKeyAttribute != null)
                    {
                        columnMetadata.IsPrimaryKey = true;
                    }
                    else
                    {
                        columnMetadata.IsPrimaryKey = false;
                    }

                    // Get the column's foreign keys.
                    PrismForeignKeyAttribute? prismForeignKeyAttribute = property.GetCustomAttribute<PrismForeignKeyAttribute>();
                    if (prismForeignKeyAttribute != null)
                    {
                        ForeignKeyMetadata foreignKeyMetadata = new ForeignKeyMetadata();

                        foreignKeyMetadata.ReferencedTableName = prismForeignKeyAttribute.ReferencedTableName;
                        foreignKeyMetadata.ReferencedColumnName = prismForeignKeyAttribute.ReferencedColumnName;

                        columnMetadata.ForeignKey = foreignKeyMetadata;
                    }
                    else
                    {
                        columnMetadata.ForeignKey = null;
                    }

                    columns.Add(columnMetadata);
                }
                else
                {
                    throw new Exception("No column definition defined.");
                }
            }

            return columns;
        }
    }
}