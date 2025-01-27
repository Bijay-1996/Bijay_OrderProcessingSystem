using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Models;
using Serilog;

namespace OrderProcessingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                Log.Information("Attempting to add a new product: {@Product}", product);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                int newid= _context.Products.Count()+1;
                Log.Information("Product successfully added with Id: {ProductId}", newid);
                return CreatedAtAction(nameof(GetProduct), new { id = newid }, product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding the product: {@Product}", product);
                return StatusCode(500, "An error occurred while adding the product.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                Log.Information("Fetching product details for ProductId: {ProductId}", id);
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    Log.Warning("Product with Id: {ProductId} not found.", id);
                    return NotFound($"Product with Id {id} not found.");
                }

                Log.Information("Product details retrieved successfully for ProductId: {ProductId}", id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching product details for ProductId: {ProductId}", id);
                return StatusCode(500, "An error occurred while fetching product details.");
            }
        }

    }
}
