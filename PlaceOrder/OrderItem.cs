namespace PlaceOrder {
    public class OrderItem {
        public int Product { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
    }
}