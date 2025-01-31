using Domain.Entities;

namespace Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment> {
    IEnumerable<Comment> GetAllByProduct(Product product);
}