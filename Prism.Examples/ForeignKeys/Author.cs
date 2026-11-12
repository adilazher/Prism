using Prism.Core.Attributes;

namespace Prism.Examples.ForeignKeys
{
    [PrismTable(Name = "Authors")]
    public class Author
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("FullName", "NVARCHAR(200)")]
        public string FullName { get; set; }

        public Author(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
    }
}
