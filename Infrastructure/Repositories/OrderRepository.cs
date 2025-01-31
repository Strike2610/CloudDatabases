using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class OrderRepository(DatabaseContext databaseContext) : Repository<Order>(databaseContext), IOrderRepository { }