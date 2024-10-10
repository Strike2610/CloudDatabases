namespace Domain;

public class Customer {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}