using System.Text.Json;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CustomObjects;

namespace QueueProcessing {
    public class ProcessShipment(ILoggerFactory loggerFactory) {
        private readonly ILogger _logger = loggerFactory.CreateLogger<ProcessShipment>();

        [Function(nameof(ProcessShipment))]
        public async Task Run([QueueTrigger("shipped-orders")] QueueMessage message) {

            TableEntity? order = null;

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var tableClient = new TableClient(connectionString, "orders");
            var orderData = JsonSerializer.Deserialize<DbOrder>(message.Body);

            if(orderData == null) {
                _logger.LogError("Order data is null, cannot process shipment.");
                return;
            }

            try {
                order = await tableClient.GetEntityAsync<TableEntity>(orderData.PartitionKey, orderData.RowKey);
                _logger.LogInformation("Order found: {}", order.RowKey);
            } catch(Exception e) {
                _logger.LogWarning(e, "Order not found");
            }

            if(order == null) {
                _logger.LogError("Order is null, cannot process shipment.");
                return;
            }
            order["ShipDate"] = message.InsertedOn ?? DateTimeOffset.MinValue;
            if(order.TryGetValue("OrderDate", out var orderDateObj) && orderDateObj != null) {
                order["OrderProcessed"] = message.InsertedOn?.Subtract(DateTimeOffset.Parse(orderDateObj.ToString()));
            } else {
                _logger.LogError("OrderDate is null, cannot process shipment.");
                return;
            }
            order.PartitionKey = "Shipped";
            await tableClient.DeleteEntityAsync("Ordered", order.RowKey);
            await tableClient.AddEntityAsync(order);
            _logger.LogInformation("Order shipped: {}", message.MessageId);
        }

        public void LogError(string message) {
            _logger.LogError(message);
            throw new Exception(message);
        }
    }
}