using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    public class ResourceGroupsController : BaseController
    {
        private IGetResourceGroupsService _getResourceGroupsService;
        private readonly ICreateResourceGroupService _createResourceGroupService;

        public ResourceGroupsController(
            IGetResourceGroupsService getResourceGroupsService, 
            ICreateResourceGroupService createResourceGroupService)
        {
            _getResourceGroupsService = getResourceGroupsService;
            _createResourceGroupService = createResourceGroupService;
        }

        [HttpGet, Route("{subscriptionId}")]
        public IEnumerable<ResourceGroup> Get(string subscriptionId)
        {
            var resourceGroups = _getResourceGroupsService.GetResourceGroups(subscriptionId);
            return resourceGroups;
        }

        [HttpPut]
        public async Task<ResourceGroup> Create(string subscriptionId, string location, string resourceGroupName)
        {
            var resourceGroup = await _createResourceGroupService.Create(subscriptionId, location, resourceGroupName);
            return resourceGroup;
        }
    }
}
