using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureVmManager.WebApi.Controllers
{
    public class ImagesController : BaseController
    {
        private IGetImagesService _getImagesService;

        public ImagesController(IGetImagesService getImagesService)
        {
            _getImagesService = getImagesService;
        }

        [HttpGet]
        public IEnumerable<Image> Get(string subscriptionId, string resourceGroupName, string galleryName)
        {
            var images = _getImagesService.GetImages(subscriptionId, resourceGroupName, galleryName);
            return images;
        }
    }
}
