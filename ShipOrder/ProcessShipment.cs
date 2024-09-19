using System.Text.Json;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PlaceOrder;

namespace ShipOrder {
    public class ProcessShipment {
        [Function(nameof(ProcessShipment))]
        public async Task Run([QueueTrigger("shipped-orders")] QueueMessage message, FunctionContext context) {
            var logger = context.GetLogger(nameof(ProcessShipment));
            TableEntity? order = null;

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var tableClient = new TableClient(connectionString, "orders");
            var orderData = JsonSerializer.Deserialize<DbOrder>(message.Body.ToString());

            if(orderData == null) {
                logger.LogError("Order data is null, cannot process shipment.");
                return;
            }
            try {
                order = await tableClient.GetEntityAsync<TableEntity>(orderData.PartitionKey, orderData.RowKey);
                logger.LogInformation("Order found: {}", order.RowKey);
            } catch(Exception e) {
                logger.LogWarning("Order not found: {}", e);
            }

            if(order == null) {
                logger.LogError("Order is null, cannot process shipment.");
                return;
            }
            order["ShipDate"] = message.InsertedOn ?? DateTimeOffset.MinValue;
            order.PartitionKey = "Shipped";
            await tableClient.DeleteEntityAsync("Ordered", order.RowKey);
            await tableClient.AddEntityAsync(order);
            logger.LogInformation("Order shipped: {}", message.MessageId);
        }
    }
}