using AzureVmManager.DataObjects;
using Elector8EnvironmentManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class GetSubscriptionsService
    {
        private IArmClientFactory _armClientFactory;

        public GetSubscriptionsService(IArmClientFactory armClientFactory)
        {
            _armClientFactory = armClientFactory;
        }

        public IEnumerable<Subscription> GetSubscriptions()
        {
            var client = _armClientFactory.CreateClient();
            var collection = client.GetSubscriptions();
            var subscriptions = collection.Select(x => new Subscription
            {
                Id = x.Data.SubscriptionId,
                Name = x.Data.DisplayName
            }).ToList();
            return subscriptions;
        }
    }
}
