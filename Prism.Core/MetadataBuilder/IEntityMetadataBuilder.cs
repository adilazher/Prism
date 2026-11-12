using Prism.Core.Metadata;

namespace Prism.Core.MetadataBuilder
{
    /// <summary>
    /// Contract for building an <see cref="EntityMetadata"/> schema description from a CLR type.
    /// Implementations inspect the type via reflection and translate Prism attributes into metadata
    /// consumed by the DDL/DML generators.
    /// </summary>
    public interface IEntityMetadataBuilder
    {
        /// <summary>
        /// Builds and returns the full schema metadata for the given entity <paramref name="type"/>.
        /// </summary>
        public EntityMetadata BuildEntityMetadata(Type type);
    }
}
