using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class GetResourceGroupsService
    {
        private readonly IGetSubscriptionResourceService _getSubscriptionResourceService;

        public GetResourceGroupsService(
            IGetSubscriptionResourceService getSubscriptionResourceService)
        {
            _getSubscriptionResourceService = getSubscriptionResourceService;
        }

        public IEnumerable<ResourceGroup> GetResourceGroups(string subscriptionId)
        {
            var subscriptionResource = _getSubscriptionResourceService.GetSubscriptionResource(subscriptionId);
            var collection = subscriptionResource.GetResourceGroups();
            var resourceGroups = collection.Select(x => new ResourceGroup
            {
                Id = x.Data.Id.ToString(),
                Location = x.Data.Location,
                Name = x.Data.Name
            }).ToList();
            return resourceGroups;
        }
    }
}
