using Azure.Identity;
using Azure.ResourceManager;
using Elector8EnvironmentManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class ArmClientFactory : IArmClientFactory
    {
        public ArmClient CreateClient()
        {
            var client = new ArmClient(new DefaultAzureCredential());
            return client;
        }
    }
}
