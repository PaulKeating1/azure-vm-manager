using Azure.ResourceManager.Resources;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetSubscriptionResourceService
    {
        SubscriptionResource GetSubscriptionResource(string subscriptionId);
    }
}
