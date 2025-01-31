using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class ProductRepository(DatabaseContext databaseContext) : Repository<Product>(databaseContext), IProductRepository { }