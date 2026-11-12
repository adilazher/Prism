namespace Prism.Core.Attributes
{
    /// <summary>
    /// Marks a class as a Prism-managed entity mapped to a database table.
    /// The <see cref="Name"/> property specifies the table name; if omitted, the class name is used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PrismTableAttribute : Attribute
    {
        public string? Name { get; set; }
    }
}
