using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    public class SubscriptionsController : BaseController
    {
        private IGetSubscriptionsService _getSubscriptionsService;

        public SubscriptionsController(IGetSubscriptionsService getSubscriptionsService)
        {
            _getSubscriptionsService = getSubscriptionsService;
        }

        [HttpGet]
        public IEnumerable<Subscription> Get()
        {
            var subscriptions = _getSubscriptionsService.GetSubscriptions();
            return subscriptions;
        }
    }
}
