using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Migrations;
using OrderProcessingSystem.Models;
using Serilog;

namespace OrderProcessingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int customerId, List<int> productIds,int Quantity)
        {
            try
            {
                Log.Information("Attempting to create an order for CustomerId: {CustomerId} with ProductIds: {@ProductIds}", customerId, productIds);

                var customer = await _context.Customers
                    .Include(c => c.Orders)
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    Log.Warning("Customer with Id: {CustomerId} not found.", customerId);
                    return NotFound("Customer not found.");
                }

                if (customer.Orders.Any(o => !o.IsFulfilled))
                {
                    Log.Warning("Customer with Id: {CustomerId} has an unfulfilled order.", customerId);
                    return BadRequest("Previous order is unfulfilled.");
                }

                // Fetch the products from the database
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                if (products.Count != productIds.Count)
                {
                    Log.Warning("Some products not found. Expected: {Expected}, Found: {Found}", productIds.Count, products.Count);
                    return BadRequest("Some products not found.");
                }

                // Check if stock is sufficient for each product
                foreach (var product in products)
                {
                    var requestedQuantity = productIds
                        .GroupBy(id => id)
                        .Where(g => g.Key == product.Id)
                        .Select(g => g.Count())
                        .FirstOrDefault();

                    if (product.StockQuantity < Quantity)
                    {
                        Log.Warning("Product with Id: {ProductId} has insufficient stock. Available: {Available}, Requested: {Requested}",
                            product.Id, product.StockQuantity, requestedQuantity);
                        return BadRequest($"Product {product.Id} does not have enough stock. Available: {product.StockQuantity}, Requested: {requestedQuantity}");
                    }
                }

                // Create the order
                var order = new Order
                {
                    CustomerId = customerId,
                    Products = products,
                    IsFulfilled = false,
                    OrderQuantity = Quantity,
                    //OrderQuantity = products.Sum(p => productIds.Count(id => id == p.Id))
                };

                _context.Orders.Add(order);

                // Update stock quantities for the ordered products
                foreach (var product in products)
                {
                    var orderedQuantity = productIds
                        .GroupBy(id => id)
                        .Where(g => g.Key == product.Id)
                        .Select(g => g.Count())
                        .FirstOrDefault();

                    product.StockQuantity -= Quantity;
                }

                await _context.SaveChangesAsync();

                Log.Information("Order created successfully for CustomerId: {CustomerId} with ProductIds: {@ProductIds}", customerId, productIds);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating an order for CustomerId: {CustomerId}", customerId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                Log.Information("Fetching order details for OrderId: {OrderId}", id);
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Products)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    Log.Warning("Order with Id: {OrderId} not found.", id);
                    return NotFound($"Order with Id {id} not found.");
                }

                Log.Information("Order details retrieved successfully for OrderId: {OrderId}", id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching order details for OrderId: {OrderId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
