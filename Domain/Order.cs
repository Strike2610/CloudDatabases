namespace Domain;

public class Order {
    public int Id { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ShipDate { get; set; }
    public TimeSpan? OrderProcessed { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
