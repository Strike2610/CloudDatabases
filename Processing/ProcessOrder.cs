using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using EntityFramework;
using Microsoft.Extensions.Logging;

namespace QueueProcessing;

public class ProcessOrder(CloudDbContext database, ILoggerFactory loggerFactory) {
    private readonly ILogger _logger = loggerFactory.CreateLogger<ProcessOrder>();

    [Function(nameof(ProcessOrder))]
    public void Run([QueueTrigger("placed-orders")] QueueMessage message) {
        var enteredData = JsonSerializer.Deserialize<OrderItem>(message.Body.ToString())!;

        var newCustomer = new Customer() {
            Name = enteredData.Customer,
            Address = enteredData.Address
        };
        var customer = database.Customers.FirstOrDefault(customer => customer.Name == newCustomer.Name && customer.Address == newCustomer.Address, newCustomer);
        if(customer == newCustomer) database.Customers.Add(customer);

        var order = new Order() {
            Product = enteredData.Product,
            CustomerId = customer.Id,
            OrderDate = message.InsertedOn ?? DateTimeOffset.MinValue
        };

        database.Orders.Add(order);
        database.SaveChanges();

        _logger.LogInformation("Order placed:  {}", order.Id);
    }
}