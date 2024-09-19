using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using PlaceOrder.UserFacing;

namespace PlaceOrder {
    public class ProcessOrder {
        [Function(nameof(ProcessOrder))]
        [TableOutput("orders")]
        public DbOrder? Run([QueueTrigger("placed-orders")] QueueMessage message) {
            if(message.Body.ToString() == "Invalid data") return null;

            var enteredData = JsonSerializer.Deserialize<OrderItem>(message.Body.ToString());
            return new DbOrder() {
                Id = message.MessageId,
                Product = enteredData.Product,
                Customer = enteredData.Customer,
                OrderDate = message.InsertedOn ?? DateTimeOffset.MinValue,
                ShipDate = DateTimeOffset.MinValue
            };
        }
    }
}
