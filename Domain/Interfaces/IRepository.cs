using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity {
    public Task<T> Update(T entity);
    public Task<T> Add(T entity);
    public Task<T?> Get(Guid id);
    public Task<bool> Exists(Guid id);
}