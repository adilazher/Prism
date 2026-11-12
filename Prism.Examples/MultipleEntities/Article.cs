using Prism.Core.Attributes;

namespace Prism.Examples.MultipleEntities
{
    [PrismTable(Name = "Articles")]
    public class Article
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Title", "NVARCHAR(300)")]
        public string Title { get; set; }

        [PrismForeignKey(ReferencedTableName = "Categories", ReferencedColumnName = "Id")]
        [PrismColumn("CategoryId", "INT")]
        public int CategoryId { get; set; }

        public Article(int id, string title, int categoryId)
        {
            Id = id;
            Title = title;
            CategoryId = categoryId;
        }
    }
}
