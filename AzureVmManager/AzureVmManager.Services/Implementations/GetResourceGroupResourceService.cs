using Azure.Core;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Interfaces;
using Elector8EnvironmentManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class GetResourceGroupResourceService : IGetResourceGroupResourceService
    {
        private IArmClientFactory _armClientFactory;

        public GetResourceGroupResourceService(IArmClientFactory armClientFactory)
        {
            _armClientFactory = armClientFactory;
        }

        public async Task<ResourceGroupResource> Get(string resourceGroupResourceId)
        {
            var armClient = _armClientFactory.CreateClient();
            var resourceGroupResource = armClient.GetResourceGroupResource(new ResourceIdentifier(resourceGroupResourceId));
            var response = await resourceGroupResource.GetAsync();
            return response.Value;
        }
    }
}
