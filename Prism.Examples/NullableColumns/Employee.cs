using Prism.Core.Attributes;

namespace Prism.Examples.NullableColumns
{
    [PrismTable(Name = "Employees")]
    public class Employee
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Name", "NVARCHAR(200)")]
        public string Name { get; set; }

        // Nullable<int> - Prism detects the nullable type and omits NOT NULL from the DDL.
        [PrismColumn("ManagerId", "INT")]
        public int? ManagerId { get; set; }

        // Nullable string - employees may not have a department assigned yet.
        [PrismColumn("Department", "NVARCHAR(100)")]
        public string? Department { get; set; }

        public Employee(int id, string name, int? managerId = null, string? department = null)
        {
            Id = id;
            Name = name;
            ManagerId = managerId;
            Department = department;
        }
    }
}
