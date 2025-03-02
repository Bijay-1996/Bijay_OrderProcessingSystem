using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderProcessingSystem.Controllers;
using OrderProcessingSystem.Models;
using OrderProcessingSystem.Services;
using Xunit;
using OrderProcessingSystem.Models;
using OrderProcessingSystem.Controllers;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Services;

namespace OrderProcessingSystem.OrderManagement.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<CustomerService> _mockCustomerService;
        private readonly CustomersController _controller;
        private readonly AppDbContext _context;

        //public CustomerControllerTests()
        //{
        //    _mockCustomerService = new Mock<CustomerService>();
        //    _controller = new CustomersController(_context,_mockCustomerService.Object);
        //}

        [Fact]
        public async Task GetCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var mockCustomers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe" },
            new Customer { Id = 2, Name = "Jane Smith" }
        };

            _mockCustomerService.Setup(service => service.GetAllCustomersAsync())
                                .ReturnsAsync(mockCustomers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCustomers_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockCustomerService.Setup(service => service.GetAllCustomersAsync())
                                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while fetching customers.", statusCodeResult.Value);
        }
    }
}
