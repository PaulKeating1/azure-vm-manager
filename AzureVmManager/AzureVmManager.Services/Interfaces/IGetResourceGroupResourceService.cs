using Azure.ResourceManager.Resources;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetResourceGroupResourceService
    {
        Task<ResourceGroupResource> Get(string resourceGroupResourceId);
    }
}
