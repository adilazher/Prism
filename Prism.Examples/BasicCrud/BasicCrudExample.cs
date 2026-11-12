using Prism.Core.ORMContext;

namespace Prism.Examples.BasicCrud
{
    // Demonstrates the core CRUD operations: Insert and GetById.
    public static class BasicCrudExample
    {
        public static void Run(string connectionString)
        {
            using PrismContext context = new PrismContext(connectionString);

            context.Delete<Product>(1);

            // Insert a new product.
            Product newProduct = new Product(1, "Keyboard", 79.99m);
            bool inserted = context.Insert(newProduct);
            Console.WriteLine($"Insert succeeded: {inserted}");

            // Fetch it back by primary key.
            Product fetched = context.GetById<Product>(1);
            Console.WriteLine($"Fetched: {fetched.Name} @ {fetched.Price:C}");
        }
    }
}
