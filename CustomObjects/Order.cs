namespace EntityFramework;

public class Order {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int Product { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset ShipDate { get; set; }
    public TimeSpan OrderProcessed { get; set; }
}
