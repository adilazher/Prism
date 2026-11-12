using Prism.Core.Attributes;

namespace Prism.Examples.IgnoredProperties
{
    [PrismTable(Name = "Customers")]
    public class Customer
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Email", "NVARCHAR(300)")]
        public string Email { get; set; }

        // PrismIgnore tells Prism to skip this property entirely -
        // no column is created for it and it is excluded from all SQL statements.
        [PrismIgnore]
        public string DisplayLabel => $"Customer #{Id}";

        public Customer(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
