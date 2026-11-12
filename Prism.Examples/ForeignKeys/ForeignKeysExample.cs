using Prism.Core.ORMContext;

namespace Prism.Examples.ForeignKeys
{
    // Demonstrates declaring a foreign key relationship between two entities.
    // Prism emits REFERENCES Authors(Id) on the Books.AuthorId column in the CREATE TABLE statement.
    public static class ForeignKeysExample
    {
        public static void Run(string connectionString)
        {
            using PrismContext context = new PrismContext(connectionString);

            context.Delete<Book>(1);
            context.Delete<Author>(1);

            // Insert the parent row first to satisfy the FK constraint.
            Author author = new Author(1, "George Orwell");
            context.Insert(author);

            // Insert a book that references the author.
            Book book = new Book(1, "Nineteen Eighty-Four", authorId: 1);
            bool inserted = context.Insert(book);
            Console.WriteLine($"Book inserted: {inserted}");

            Book fetched = context.GetById<Book>(1);
            Console.WriteLine($"Fetched book: {fetched.Title} (AuthorId={fetched.AuthorId})");
        }
    }
}
