using Prism.Core.Attributes;

namespace Prism.Examples.MultipleEntities
{
    [PrismTable(Name = "Categories")]
    public class Category
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Label", "NVARCHAR(100)")]
        public string Label { get; set; }

        public Category(int id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
