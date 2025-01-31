using Azure.Storage.Queues.Models;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using System.Text.Json;

namespace Application.Processing;

public interface IProcessOrderComponent {
    Task Execute(QueueMessage message);
}

public class ProcessOrderComponent(ICustomerRepository customerRepository, IOrderRepository orderRepository) : IProcessOrderComponent {
    public async Task Execute(QueueMessage message) {
        var enteredData = JsonSerializer.Deserialize<OrderDto>(message.Body.ToString())!;
        var customer = await customerRepository.GetOrCreateCustomer(enteredData.Name, enteredData.Address);

        var newCustomer = new Customer {
            Name = enteredData.Name,
            Address = enteredData.Address
        };

        if(customer == newCustomer) await customerRepository.Add(customer);

        var order = new Order {
            ProductId = enteredData.Product,
            CustomerId = customer.Id,
            OrderDate = message.InsertedOn ?? DateTimeOffset.MinValue
        };

        await orderRepository.Add(order);
    }
}