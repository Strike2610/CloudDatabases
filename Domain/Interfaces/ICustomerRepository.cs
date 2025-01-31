using Domain.Entities;

namespace Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer> {
    Task<Customer> GetOrCreateCustomer(string name, string address);
}