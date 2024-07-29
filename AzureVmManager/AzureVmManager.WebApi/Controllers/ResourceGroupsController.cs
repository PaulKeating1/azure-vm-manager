using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceGroupsController : ControllerBase
    {
        private IGetResourceGroupsService _getResourceGroupsService;

        public ResourceGroupsController(IGetResourceGroupsService getResourceGroupsService)
        {
            _getResourceGroupsService = getResourceGroupsService;
        }

        [HttpGet]
        public IEnumerable<ResourceGroup> Get(string subscriptionId)
        {
            var resourceGroups = _getResourceGroupsService.GetResourceGroups(subscriptionId);
            return resourceGroups;
        }
    }
}
