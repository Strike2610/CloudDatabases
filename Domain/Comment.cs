namespace Domain;

public class Comment {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PostDate { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}