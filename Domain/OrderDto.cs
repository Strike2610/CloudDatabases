namespace Domain;

public interface IOrderDto {
    public int Product { get; set; }
    public string Customer { get; set; }
    public string Address { get; set; }
}

internal class OrderDto : IOrderDto {
    public int Product { get; set; }
    public required string Customer { get; set; }
    public required string Address { get; set; }
}