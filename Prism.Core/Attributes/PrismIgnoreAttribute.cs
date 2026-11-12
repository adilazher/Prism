namespace Prism.Core.Attributes
{
    /// <summary>
    /// Excludes a property or class from Prism processing entirely.
    /// On a property, the column is omitted from metadata and SQL generation.
    /// On a class, the entity is skipped during assembly discovery.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PrismIgnoreAttribute : Attribute
    {

    }
}
