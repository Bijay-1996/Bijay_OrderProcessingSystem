using OnvifDiscovery.Models;
using OnvifDiscovery;
using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Services
{
    public class OnvifDiscoveryService : IOnvifDiscoveryService
    {
        public List<OnvifDevice> DiscoverDevices(int timeoutSeconds)
        {
            List<OnvifDevice> onvifDevices = new List<OnvifDevice>();
            var discoveredDevices = new Discovery().Discover(timeoutSeconds, CancellationToken.None);

            foreach (DiscoveryDevice device in discoveredDevices.Result)
            {
                onvifDevices.Add(new OnvifDevice
                {
                    Address = device.Address,
                    Manufacturer = device.Mfr,
                    Model = device.Model,
                    OnvifServiceUrl = Convert.ToString(device.XAddresses)
                });
            }

            return onvifDevices;
        }
    }
}
