using Prism.Core.ORMContext;

namespace Prism.Examples.MultipleEntities
{
    // Demonstrates a single PrismContext managing multiple entity types at once.
    // On construction, Prism discovers all PrismTable-annotated classes in the assembly
    // and creates their tables in one transactional migration - no per-entity setup needed.
    public static class MultipleEntitiesExample
    {
        public static void Run(string connectionString)
        {
            // One context, two tables (Categories + Articles) created automatically.
            using PrismContext context = new PrismContext(connectionString);

            context.Delete<Article>(1);
            context.Delete<Category>(1);

            Category category = new Category(1, "Technology");
            context.Insert(category);

            Article article = new Article(1, "Introduction to ORMs", categoryId: 1);
            context.Insert(article);

            Category fetchedCategory = context.GetById<Category>(1);
            Article fetchedArticle = context.GetById<Article>(1);

            Console.WriteLine($"Category: {fetchedCategory.Label}");
            Console.WriteLine($"Article: {fetchedArticle.Title} (CategoryId={fetchedArticle.CategoryId})");
        }
    }
}
