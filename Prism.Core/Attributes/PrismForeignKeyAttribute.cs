namespace Prism.Core.Attributes
{
    /// <summary>
    /// Declares a property as a foreign key column referencing another table.
    /// <see cref="ReferencedTableName"/> and <see cref="ReferencedColumnName"/> identify the target table and column
    /// used when the DDL generator emits the FOREIGN KEY constraint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrismForeignKeyAttribute : Attribute
    {
        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }
    }
}
