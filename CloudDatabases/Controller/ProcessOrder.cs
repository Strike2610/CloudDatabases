using System.Text.Json;
using Azure.Storage.Queues.Models;
using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;

namespace CloudDatabases.Controller;

public class ProcessOrder(CloudContext database) {
    [Function(nameof(ProcessOrder))]
    public void Run([QueueTrigger("placed-orders")] QueueMessage message) {
        var enteredData = JsonSerializer.Deserialize<OrderItem>(message.Body.ToString())!;

        var newCustomer = new Customer() {
            Name = enteredData.Customer,
            Address = enteredData.Address
        };
        var customer = database.Customers.AsEnumerable().FirstOrDefault(customer => customer.Name == newCustomer.Name && customer.Address == newCustomer.Address, newCustomer);
        if(customer == newCustomer) {
            database.Customers.Add(customer);
            database.SaveChanges();
        }

        var order = new Order() {
            ProductId = enteredData.Product,
            CustomerId = customer.Id,
            OrderDate = message.InsertedOn ?? DateTimeOffset.MinValue
        };

        database.Orders.Add(order);
        database.SaveChanges();
    }
}