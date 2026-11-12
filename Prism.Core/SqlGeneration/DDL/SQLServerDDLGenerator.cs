using System.Text;
using Prism.Core.Metadata;

namespace Prism.Core.SqlGeneration.DDL
{
    /// <summary>
    /// SQL Server implementation of <see cref="IDDLGenerator"/>.
    /// Builds <c>CREATE TABLE</c> statements using a <see cref="StringBuilder"/> by walking each
    /// <see cref="ColumnMetadata"/> entry and appending inline constraints (NOT NULL, PRIMARY KEY,
    /// REFERENCES) in the order expected by T-SQL.
    /// </summary>
    public class SQLServerDDLGenerator : IDDLGenerator
    {
        public string GenerateCreateTableStatement(EntityMetadata entityMetadata)
        {
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append($"CREATE TABLE {entityMetadata.TableName} (");

            foreach (ColumnMetadata item in entityMetadata.Columns)
            {
                commandBuilder.Append($"{item.Name} {item.DataType} ");

                if (item.IsNullable == false)
                {
                    commandBuilder.Append("NOT NULL ");
                }

                if (item.IsPrimaryKey)
                {
                    commandBuilder.Append("PRIMARY KEY ");
                }

                if (item.ForeignKey != null)
                {
                    commandBuilder.Append($"REFERENCES {item.ForeignKey.ReferencedTableName}({item.ForeignKey.ReferencedColumnName})");
                }

                commandBuilder.Append(", ");
            }

            // Strip the trailing ", " left by the last column before closing the statement.
            if (commandBuilder.ToString().EndsWith(", "))
                commandBuilder.Remove(commandBuilder.Length - 2, 2);

            return commandBuilder.Append(");").ToString();
        }
    }
}
