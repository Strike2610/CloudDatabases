using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T>(DatabaseContext databaseContext) : IRepository<T> where T : BaseEntity {
    protected readonly DatabaseContext DatabaseContext = databaseContext;

    public async Task<T> Add(T entity) {
        await DatabaseContext.AddAsync(entity);

        return entity;
    }

    public async Task<T?> Get(Guid id) {
        return await DatabaseContext.Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<T> Update(T entity) {
        DatabaseContext.Update(entity);

        return Task.FromResult(entity);
    }

    public async Task<bool> Exists(Guid id) { return await DatabaseContext.Set<T>().AnyAsync(g => g.Id == id); }

    public async Task SaveChanges() { await DatabaseContext.SaveChangesAsync(); }
}