namespace Domain.Entities;

public class Order : BaseEntity {
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ShipDate { get; set; }
    public TimeSpan? OrderProcessed { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}