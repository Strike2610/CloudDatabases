using Azure;
using Azure.Data.Tables;

namespace CustomObjects;

public record DbOrder : ITableEntity {
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public int Product { get; set; }
    public required string Customer { get; set; }
    public required string Address { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ShipDate { get; set; }
}