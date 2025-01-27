using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Services;
using Serilog;

namespace OrderProcessingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CustomerService _customerService;

        public CustomersController(AppDbContext context, CustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                Log.Information("Fetching all customers.");
                var customers = await _customerService.GetAllCustomersAsync();
                Log.Information("Successfully retrieved {CustomerCount} customers.", customers?.Count ?? 0);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching customers.");
                return StatusCode(500, "An error occurred while fetching customers.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                Log.Information("Fetching details for CustomerId: {CustomerId}", id);
                var customer = await _context.Customers
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (customer == null)
                {
                    Log.Warning("Customer with Id: {CustomerId} not found.", id);
                    return NotFound($"Customer with Id {id} not found.");
                }

                Log.Information("Successfully retrieved details for CustomerId: {CustomerId}", id);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching details for CustomerId: {CustomerId}", id);
                return StatusCode(500, "An error occurred while fetching customer details.");
            }
        }

    }

}
