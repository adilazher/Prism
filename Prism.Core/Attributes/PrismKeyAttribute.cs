namespace Prism.Core.Attributes
{
    /// <summary>
    /// Marks a property as the primary key column for its entity.
    /// Must be combined with <see cref="PrismColumnAttribute"/>. Only one primary key per entity is supported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrismKeyAttribute : Attribute
    {
    }
}
