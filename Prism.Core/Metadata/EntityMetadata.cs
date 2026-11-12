namespace Prism.Core.Metadata
{
    /// <summary>
    /// Represents the runtime schema description of a single Prism-managed entity.
    /// Built by <c>EntityMetadataBuilder</c> from reflection and used by the DDL/DML generators
    /// to produce SQL without re-inspecting the type at query time.
    /// </summary>
    public class EntityMetadata
    {
        public Type EntityType { get; set; }
        public string TableName { get; set; }
        public List<ColumnMetadata> Columns { get; set; }
    }
}
