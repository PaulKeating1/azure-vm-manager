using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetSubscriptionsService
    {
        IEnumerable<Subscription> GetSubscriptions();
    }
}
