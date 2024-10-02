using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using EntityFramework;

namespace QueueProcessing;

public class ProcessShipment(CloudDbContext database) {
    [Function(nameof(ProcessShipment))]
    public void Run([QueueTrigger("shipped-orders")] QueueMessage message) {
        var orderId = int.Parse(message.Body.ToString());
        var order = database.Orders.First(order => order.Id == orderId);

        order.ShipDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        order.OrderProcessed = order.ShipDate.Subtract(order.OrderDate);
        database.Orders.Update(order);
    }
}