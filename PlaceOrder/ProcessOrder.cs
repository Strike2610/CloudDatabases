using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PlaceOrder.UserFacing;

namespace PlaceOrder {
    public class ProcessOrder {
        [Function(nameof(ProcessOrder))]
        [TableOutput("orders")]
        public DbOrder? Run([QueueTrigger("placed-orders")] QueueMessage message, FunctionContext context) {
            if(message.Body.ToString() == "Invalid data") return null;
            var logger = context.GetLogger(nameof(ProcessOrder));

            var enteredData = JsonSerializer.Deserialize<OrderItem>(message.Body.ToString());
            logger.LogInformation("Order saved: {}", message.MessageId);
#pragma warning disable CS8602 // Dereference of a possibly null reference. Irrelevant due to only internal data being used.
            return new DbOrder() {
                PartitionKey = "Ordered",
                RowKey = Guid.NewGuid().ToString(),
                Product = enteredData.Product,
                Customer = enteredData.Customer,
                Address = enteredData.Address,
                OrderDate = message.InsertedOn ?? DateTimeOffset.MinValue,
                ShipDate = null
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
