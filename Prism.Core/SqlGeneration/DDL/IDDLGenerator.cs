using Prism.Core.Metadata;

namespace Prism.Core.SqlGeneration.DDL
{
    /// <summary>
    /// Contract for generating DDL (Data Definition Language) SQL statements from entity metadata.
    /// Implementations are database-vendor specific; swap the implementation to target a different engine.
    /// </summary>
    public interface IDDLGenerator
    {
        /// <summary>
        /// Produces a <c>CREATE TABLE</c> statement for the table described by <paramref name="entityMetadata"/>,
        /// including column definitions, nullability, primary key, and foreign key constraints.
        /// </summary>
        public string GenerateCreateTableStatement(EntityMetadata entityMetadata);
    }
}
