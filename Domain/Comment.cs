namespace Domain;

public interface IComment {
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PostDate { get; set; }
    public int ProductId { get; set; }
    public IProduct Product { get; set; }
}

internal class Comment : IComment {
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTimeOffset PostDate { get; set; }
    public int ProductId { get; set; }
    public required IProduct Product { get; set; }
}