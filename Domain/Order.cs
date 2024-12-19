namespace Domain;

public interface IOrder {
    public int Id { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ShipDate { get; set; }
    public TimeSpan? OrderProcessed { get; set; }
    public int CustomerId { get; set; }
    public ICustomer Customer { get; set; }
    public int ProductId { get; set; }
    public IProduct Product { get; set; }
}

public class Order : IOrder {
    public int Id { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ShipDate { get; set; }
    public TimeSpan? OrderProcessed { get; set; }
    public int CustomerId { get; set; }
    public required ICustomer Customer { get; set; }
    public int ProductId { get; set; }
    public required IProduct Product { get; set; }
}