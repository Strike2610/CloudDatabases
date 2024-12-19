namespace Domain;

public interface IProduct {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Thumbnail { get; set; }
    public ICollection<IComment> Comments { get; }
}

internal class Product : IProduct {
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Thumbnail { get; set; }
    public ICollection<IComment> Comments { get; } = new List<IComment>();
}