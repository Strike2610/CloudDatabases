namespace Domain;

public interface ICustomer {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<IOrder> Orders { get; }
}

public class Customer : ICustomer {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public ICollection<IOrder> Orders { get; } = new List<IOrder>();
}