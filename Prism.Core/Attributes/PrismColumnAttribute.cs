namespace Prism.Core.Attributes
{
    /// <summary>
    /// Maps a property to a specific database column.
    /// <see cref="Name"/> sets the column name and <see cref="DataType"/> sets the SQL data type (e.g. "NVARCHAR(255)").
    /// Every non-ignored property on a <see cref="PrismTableAttribute"/>-annotated class must carry this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrismColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public PrismColumnAttribute(string name, string dataType)
        {
            Name = name;
            DataType = dataType;
        }
    }
}
