namespace Domain;

public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Thumbnail { get; set; }

    public ICollection<Comment> Comments { get; } = new List<Comment>();
}