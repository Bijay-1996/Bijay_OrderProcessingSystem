using System.Text.Json.Serialization;

namespace OrderProcessingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
