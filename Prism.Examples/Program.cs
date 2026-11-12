using Prism.Examples.BasicCrud;
using Prism.Examples.ForeignKeys;
using Prism.Examples.NullableColumns;
using Prism.Examples.IgnoredProperties;
using Prism.Examples.MultipleEntities;

internal class Program
{
    private static void Main(string[] args)
    {
        // Replace with a valid SQL Server connection string before running.
        const string connectionString = "Server=DESKTOP-4KMST42\\SQLEXPRESS;Database=exampledbmy;Trusted_Connection=True;TrustServerCertificate=True;";

        Console.WriteLine("=== 01 Basic Crud ===");
        BasicCrudExample.Run(connectionString);

        Console.WriteLine("\n=== 02 Foreign Keys ===");
        ForeignKeysExample.Run(connectionString);

        Console.WriteLine("\n=== 03 Nullable Columns ===");
        NullableColumnsExample.Run(connectionString);

        Console.WriteLine("\n=== 04 Ignored Properties ===");
        IgnoredPropertiesExample.Run(connectionString);

        Console.WriteLine("\n=== 05 Multiple Entities ===");
        MultipleEntitiesExample.Run(connectionString);
    }
}