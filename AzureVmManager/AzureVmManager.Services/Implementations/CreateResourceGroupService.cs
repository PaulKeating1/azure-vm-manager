using Azure;
using Azure.ResourceManager.Resources;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class CreateResourceGroupService : ICreateResourceGroupService
    {
        private IGetSubscriptionResourceService _getSubscriptionResourceService;

        public CreateResourceGroupService(IGetSubscriptionResourceService getSubscriptionResourceService)
        {
            _getSubscriptionResourceService = getSubscriptionResourceService;
        }

        public async Task<ResourceGroup> Create(string subscriptionId, string location, string resourceGroupName)
        {
            var subscriptionResource = _getSubscriptionResourceService.GetSubscriptionResource(subscriptionId);
            var resourceGroups = subscriptionResource.GetResourceGroups();
            var operation = await resourceGroups.CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, new ResourceGroupData(location));
            var resourceGroup = new ResourceGroup
            {
                Id = operation.Value.Data.Id.ToString(),
                Location = operation.Value.Data.Location,
                Name = operation.Value.Data.Name,
                SubscriptionName = subscriptionResource.Data.DisplayName
            };
            return resourceGroup;
        }
    }
}
