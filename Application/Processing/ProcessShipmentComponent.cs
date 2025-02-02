using Azure.Storage.Queues.Models;
using Domain.Interfaces;

namespace Application.Processing;

public interface IProcessShipmentComponent {
    Task Execute(QueueMessage message);
}

public class ProcessShipmentComponent(IOrderRepository orderRepository) : IProcessShipmentComponent {
    public async Task Execute(QueueMessage message) {
        var orderId = Guid.Parse(message.Body.ToString());
        var order = await orderRepository.Get(orderId);

        if(order == null) return;

        order.ShipDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        order.OrderProcessed = order.ShipDate!.Value.Subtract(order.OrderDate);
        await orderRepository.Update(order);
        await orderRepository.SaveChanges();
    }
}