using Prism.Core.Attributes;

namespace Prism.Examples.IgnoredProperties
{
    // PrismIgnore on the class itself tells Prism to skip the entire entity during
    // assembly discovery - no table is created and no SQL is ever generated for it.
    [PrismIgnore]
    [PrismTable(Name = "AuditLogs")]
    public class InternalAuditLog
    {
        [PrismKey]
        [PrismColumn("Id", "INT")]
        public int Id { get; set; }

        [PrismColumn("Message", "NVARCHAR(500)")]
        public string Message { get; set; }

        public InternalAuditLog() { }
    }
}
