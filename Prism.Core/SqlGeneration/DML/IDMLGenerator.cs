namespace Prism.Core.SqlGeneration.DML
{
    /// <summary>
    /// Contract for generating DML (Data Manipulation Language) SQL statements.
    /// Each method accepts pre-extracted entity data rather than a typed entity so the interface
    /// stays decoupled from the metadata layer. Implementations are database-vendor specific.
    /// </summary>
    public interface IDMLGenerator
    {
        /// <summary>
        /// Produces an <c>INSERT INTO</c> statement for the given column-value pairs and table name.
        /// </summary>
        public string GenerateInsertStatement(Dictionary<string, object> entityData, string tableName);

        /// <summary>
        /// Produces an <c>UPDATE</c> statement for the given column-value pairs and table name.
        /// </summary>
        public string GenerateUpdateStatement(Dictionary<string, object> entityData, string tableName);

        /// <summary>
        /// Produces a <c>DELETE FROM</c> statement for the given column-value pairs and table name.
        /// </summary>
        public string GenerateDeleteStatement(Dictionary<string, object> entityData, string tableName);

        /// <summary>
        /// Produces a <c>SELECT *</c> statement that filters by primary key <paramref name="id"/>.
        /// </summary>
        public string GenerateGetByIdStatement(int id, string tableName);
    }
}
