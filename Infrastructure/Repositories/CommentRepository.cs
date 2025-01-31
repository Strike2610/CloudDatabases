using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class CommentRepository(DatabaseContext databaseContext) : Repository<Comment>(databaseContext), ICommentRepository {
    public IEnumerable<Comment> GetAllByProduct(Product product) => DatabaseContext.Comments.Where(comment => comment.ProductId == product.Id);
}