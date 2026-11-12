using System.Text;

namespace Prism.Core.SqlGeneration.DML
{
    /// <summary>
    /// SQL Server implementation of <see cref="IDMLGenerator"/>.
    /// Builds DML statements as raw SQL strings using <see cref="StringBuilder"/>.
    /// String values are wrapped in single quotes; all other types are written as-is.
    /// Primary key is assumed to be named "Id" across all statements that require a WHERE clause.
    /// </summary>
    public class SQLServerDMLGenerator : IDMLGenerator
    {
        public string GenerateInsertStatement(Dictionary<string, object> entityData, string tableName)
        {
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append($"INSERT INTO {tableName} (");

            foreach (string dataColNames in entityData.Keys)
            {
                commandBuilder.Append($"{dataColNames}, ");
            }

            // Strip the trailing ", " before closing the column list.
            if (commandBuilder.ToString().EndsWith(", "))
                commandBuilder.Remove(commandBuilder.Length - 2, 2);

            commandBuilder.Append(" ) VALUES (");

            foreach (object data in entityData.Values)
            {
                if (data == null)
                {
                    commandBuilder.Append("NULL, ");
                }
                else if (data is string)
                {
                    commandBuilder.Append($"'{data.ToString()}', ");
                }
                else
                {
                    commandBuilder.Append($"{data.ToString()}, ");
                }
            }

            // Strip the trailing ", " before closing the values list.
            if (commandBuilder.ToString().EndsWith(", "))
                commandBuilder.Remove(commandBuilder.Length - 2, 2);

            commandBuilder.Append(")");

            return commandBuilder.ToString();
        }

        // Primary key column is assumed to be named "Id" - consistent with GetById convention.
        public string GenerateUpdateStatement(Dictionary<string, object> entityData, string tableName)
        {
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append($"UPDATE {tableName} SET ");

            foreach (KeyValuePair<string, object> entry in entityData)
            {
                // Skip the primary key - it belongs in the WHERE clause, not the SET list.
                if (entry.Key == "Id") { continue; }

                if (entry.Value == null)
                {
                    commandBuilder.Append($"{entry.Key} = NULL, ");
                }
                else if (entry.Value is string)
                {
                    commandBuilder.Append($"{entry.Key} = '{entry.Value.ToString()}', ");
                }
                else
                {
                    commandBuilder.Append($"{entry.Key} = {entry.Value.ToString()}, ");
                }
            }

            // Strip the trailing ", " before the WHERE clause.
            if (commandBuilder.ToString().EndsWith(", "))
                commandBuilder.Remove(commandBuilder.Length - 2, 2);

            commandBuilder.Append($" WHERE Id = {entityData["Id"]}");

            return commandBuilder.ToString();
        }

        // Primary key column is assumed to be named "Id" - consistent with GetById convention.
        public string GenerateDeleteStatement(Dictionary<string, object> entityData, string tableName)
        {
            return $"DELETE FROM {tableName} WHERE Id = {entityData["Id"]}";
        }

        // Primary key column is assumed to be named "Id" - no metadata lookup is performed here.
        public string GenerateGetByIdStatement(int id, string tableName)
        {
            return $"SELECT * FROM {tableName} WHERE Id = {id}";
        }
    }
}
