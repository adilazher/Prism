using Prism.Core.ORMContext;

namespace Prism.Examples.NullableColumns
{
    // Demonstrates nullable property types. Prism detects Nullable<T> via reflection and
    // omits the NOT NULL constraint from those columns in the generated CREATE TABLE.
    public static class NullableColumnsExample
    {
        public static void Run(string connectionString)
        {
            using PrismContext context = new PrismContext(connectionString);

            context.Delete<Employee>(1);
            context.Delete<Employee>(2);

            // ManagerId and Department are null - their columns allow NULL in the schema.
            Employee junior = new Employee(1, "Alice");
            context.Insert(junior);

            // ManagerId is set; Department is still unassigned.
            Employee senior = new Employee(2, "Bob", managerId: 1);
            context.Insert(senior);

            Employee fetched = context.GetById<Employee>(1);
            Console.WriteLine($"Name: {fetched.Name}, Manager: {fetched.ManagerId?.ToString() ?? "none"}");
        }
    }
}
