using Prism.Core.Attributes;

namespace Prism.Examples.ForeignKeys
{
    [PrismTable(Name = "Books")]
    public class Book
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Title", "NVARCHAR(300)")]
        public string Title { get; set; }

        // AuthorId references Authors(Id) - Prism emits a FOREIGN KEY constraint in the CREATE TABLE.
        [PrismForeignKey(ReferencedTableName = "Authors", ReferencedColumnName = "Id")]
        [PrismColumn("AuthorId", "INT")]
        public int AuthorId { get; set; }

        public Book(int id, string title, int authorId)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
        }
    }
}
