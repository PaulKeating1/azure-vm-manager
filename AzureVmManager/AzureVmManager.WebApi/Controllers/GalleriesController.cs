using Azure.ResourceManager.Resources;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    public class GalleriesController : BaseController
    {
        private ICreateGalleryService _createGalleryService;

        public GalleriesController(ICreateGalleryService createGalleryService)
        {
            _createGalleryService = createGalleryService;
        }

        [HttpPut]
        public async Task<Gallery> Create(string subscriptionId, string resourceGroupName, string galleryName)
        {
            var resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);
            var gallery = await _createGalleryService.Create(resourceGroupResourceId.ToString(), galleryName);
            return gallery;
        }
    }
}
