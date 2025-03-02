using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Models;
using OrderProcessingSystem.Services;

namespace OrderProcessingSystem.Controllers
{
    public class OnvifController : Controller
    {
        private readonly IOnvifDiscoveryService _onvifDiscoveryService;
        public OnvifController(IOnvifDiscoveryService onvifDiscoveryService)
        {
            _onvifDiscoveryService = onvifDiscoveryService;
        }
        [HttpGet("discover")]
        public ActionResult<List<OnvifDevice>> DiscoverDevices([FromQuery] int timeoutSeconds = 5)
        {
            try
            {
                var devices = _onvifDiscoveryService.DiscoverDevices(timeoutSeconds);

                if (devices == null || devices.Count == 0)
                {
                    return NotFound("No ONVIF devices found.");
                }

                return Ok(devices);
            }
            catch (TimeoutException ex)
            {
                return StatusCode(408, $"Discovery timed out: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return StatusCode(499, $"Operation canceled: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log error (you can use ILogger here)
                Console.WriteLine($"Error in DiscoverDevices: {ex.Message}");

                return StatusCode(500, "An unexpected error occurred while discovering ONVIF devices.");
            }
        }

    }
}
