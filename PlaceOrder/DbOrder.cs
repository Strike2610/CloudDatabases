using Azure;
using Azure.Data.Tables;

namespace PlaceOrder {
    public record DbOrder : ITableEntity {
        public required string PartitionKey { get; set; }
        public required string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public int Product { get; init; }
        public required string Customer { get; init; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset? ShipDate { get; set; }
        public required string Address { get; set; }
    }
}
