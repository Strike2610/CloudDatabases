using Azure;
using Azure.Data.Tables;

namespace CustomObjects;

public record DbProduct : ITableEntity {
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public Guid Thumbnail { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}