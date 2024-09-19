using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PlaceOrder.UserFacing;

namespace PlaceOrder {
    public class ProcessOrder {
        [Function(nameof(ProcessOrder))]
        [TableOutput("orders")]
        public TableEntity Run([QueueTrigger("placed-orders")] QueueMessage message, FunctionContext context) {
            if(message.Body.ToString() == "Invalid data") return null;
            var logger = context.GetLogger(nameof(ProcessOrder));

            var enteredData = JsonSerializer.Deserialize<OrderItem>(message.Body.ToString());
            logger.LogInformation("Order saved: {}", message.MessageId);

            var order = new TableEntity("Ordered", message.MessageId) {
                {"Product", enteredData.Product},
                {"Customer", enteredData.Customer},
                {"Address", enteredData.Address},
                {"OrderDate", message.InsertedOn ?? DateTimeOffset.MinValue},
                {"ShipDate", null}
            };
            return order;
        }
    }
}
