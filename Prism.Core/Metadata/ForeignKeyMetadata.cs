namespace Prism.Core.Metadata
{
    /// <summary>
    /// Holds the target table and column names for a foreign key constraint,
    /// attached to a <see cref="ColumnMetadata"/> when the corresponding property
    /// carries a <c>PrismForeignKeyAttribute</c>.
    /// </summary>
    public class ForeignKeyMetadata
    {
        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }
    }
}
