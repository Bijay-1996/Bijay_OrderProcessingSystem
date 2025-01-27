using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using OrderProcessingSystem.Models;

namespace OrderManagement.Tests
{
    public class OrderTests
    {
        [Fact]
        public void CalculateTotalPrice_ShouldReturnCorrectTotal()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000.0M },
                new Product { Id = 2, Name = "Mouse", Price = 50.0M }
            };
            var order = new Order { Products = products };

            // Act
            var totalPrice = order.TotalPrice;

            // Assert
            Assert.Equal(1050.0M, totalPrice);
        }

        [Fact]
        public void PreventOrderIfPreviousOrderUnfulfilled_ShouldReturnFalse()
        {
            // Arrange
            var previousOrders = new List<Order>
            {
                new Order { Id = 1, IsFulfilled = false }
            };
            var customer = new Customer { Orders = previousOrders };

            // Act
            var canPlaceOrder = !customer.Orders.Any(o => !o.IsFulfilled);

            // Assert
            Assert.False(canPlaceOrder);
        }


    }
}
