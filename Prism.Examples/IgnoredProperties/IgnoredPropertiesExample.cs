using Prism.Core.ORMContext;

namespace Prism.Examples.IgnoredProperties
{
    // Demonstrates two uses of PrismIgnoreAttribute:
    //   1. On a property  - excludes a computed/helper property from the schema and SQL.
    //   2. On a class     - excludes an entire entity from discovery so no table is created.
    public static class IgnoredPropertiesExample
    {
        public static void Run(string connectionString)
        {
            using PrismContext context = new PrismContext(connectionString);

            context.Delete<Customer>(1);

            // Only a Customers table is created - AuditLogs is skipped because the
            // class carries PrismIgnoreAttribute.
            Customer customer = new Customer(1, "alice@example.com");
            context.Insert(customer);

            Customer fetched = context.GetById<Customer>(1);

            // DisplayLabel is a computed property excluded from the DB -
            // it is populated from in-memory data, not read from a column.
            Console.WriteLine(fetched.DisplayLabel);
        }
    }
}
