using Azure.Storage.Queues.Models;
using DAL;
using Microsoft.Azure.Functions.Worker;

namespace CloudDatabases.Controller;

public class ProcessShipment(CloudContext database) {
    [Function(nameof(ProcessShipment))]
    public async Task RunAsync([QueueTrigger("shipped-orders")] QueueMessage message) {
        var orderId = int.Parse(message.Body.ToString());
        var order = database.Orders.First(order => order.Id == orderId);

        order.ShipDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        order.OrderProcessed = order.ShipDate!.Value.Subtract(order.OrderDate);
        database.Orders.Update(order);
        await database.SaveChangesAsync();
    }
}