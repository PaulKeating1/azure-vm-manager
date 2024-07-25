using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetResourceGroupsService
    {
        IEnumerable<ResourceGroup> GetResourceGroups(string subscriptionId);
    }
}
