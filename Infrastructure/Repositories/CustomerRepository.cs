using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository(DatabaseContext databaseContext) : Repository<Customer>(databaseContext), ICustomerRepository {
    public async Task<Customer> GetOrCreateCustomer(string name, string address) {
        var customer = await DatabaseContext.Customers.SingleOrDefaultAsync(customer => customer.Name == name && customer.Address == address);

        if(customer != null) return customer;

        customer = new Customer {
            Name = name,
            Address = address
        };

        return await Add(customer);
    }
}