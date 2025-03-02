using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Services
{
    public interface IOnvifDiscoveryService
    {
        List<OnvifDevice> DiscoverDevices(int timeoutSeconds);
    }

}
