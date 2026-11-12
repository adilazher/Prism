using Prism.Core.Attributes;

namespace Prism.Examples.BasicCrud
{
    [PrismTable(Name = "Products")]
    public class Product
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Name", "NVARCHAR(200)")]
        public string Name { get; set; }

        [PrismColumn("Price", "DECIMAL(10,2)")]
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
