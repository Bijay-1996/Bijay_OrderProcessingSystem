namespace OrderProcessingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public bool IsFulfilled { get; set; }
        public int OrderQuantity {  get; set; }
        public decimal TotalPrice => Products.Sum(p => p.Price * OrderQuantity);

    }
}
