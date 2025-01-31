namespace Domain.Entities;

public class Comment : BaseEntity {
    public string Content { get; set; }
    public DateTimeOffset PostDate { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}