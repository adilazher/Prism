namespace Prism.Core.Metadata
{
    /// <summary>
    /// Describes a single column within an entity's table, including its SQL name, data type,
    /// nullability, whether it is the primary key, and an optional foreign key reference.
    /// </summary>
    public class ColumnMetadata
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public ForeignKeyMetadata? ForeignKey { get; set; }
    }
}
