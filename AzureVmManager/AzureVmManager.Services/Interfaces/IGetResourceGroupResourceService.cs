using Azure.ResourceManager.Resources;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetResourceGroupResourceService
    {
        ResourceGroupResource Get(string resourceGroupResourceId);
    }
}
