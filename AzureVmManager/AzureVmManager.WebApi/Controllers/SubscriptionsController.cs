using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
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
