using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Interfaces;
using Elector8EnvironmentManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class GetSubscriptionResourceService : IGetSubscriptionResourceService
    {
        private IArmClientFactory _armClientFactory;

        public GetSubscriptionResourceService(IArmClientFactory armClientFactory)
        {
            _armClientFactory = armClientFactory;
        }

        public SubscriptionResource GetSubscriptionResource(string subscriptionId)
        {
            var armClient = _armClientFactory.CreateClient();
            var resourceIdentifier = SubscriptionResource.CreateResourceIdentifier(subscriptionId);
            var subscriptionResource = armClient.GetSubscriptionResource(resourceIdentifier);
            subscriptionResource = subscriptionResource.Get();
            return subscriptionResource;
        }
    }
}
