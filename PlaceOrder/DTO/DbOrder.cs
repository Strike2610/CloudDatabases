using Azure;
using Azure.Data.Tables;

namespace PlaceOrder.DTO
{
    public record DbOrder : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Id { get; init; }
        public int Product { get; init; }
        public string Customer { get; init; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset? ShipDate { get; set; }
    }
}
