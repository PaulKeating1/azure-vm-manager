using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface ICreateResourceGroupService
    {
        Task<ResourceGroup> Create(string subscriptionId, string location, string resourceGroupName);
    }
}
