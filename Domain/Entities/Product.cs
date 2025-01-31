namespace Domain.Entities;

public class Product : BaseEntity {
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Thumbnail { get; set; }
    public ICollection<Comment> Comments { get; }
}