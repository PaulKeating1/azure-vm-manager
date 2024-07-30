using Azure;
using Azure.ResourceManager.Compute;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class CreateGalleryService : ICreateGalleryService
    {
        private IGetResourceGroupResourceService _getResourceGroupResourceService;

        public CreateGalleryService(IGetResourceGroupResourceService getResourceGroupResourceService)
        {
            _getResourceGroupResourceService = getResourceGroupResourceService;
        }

        public async Task<Gallery> Create(string resourceGroupResourceId, string galleryName)
        {
            var resourceGroupResource = await _getResourceGroupResourceService.Get(resourceGroupResourceId);
            var galleriesCollection = resourceGroupResource.GetGalleries();
            var armOperation = await galleriesCollection.CreateOrUpdateAsync(WaitUntil.Completed, galleryName, new GalleryData(resourceGroupResource.Data.Location));
            var gallery = new Gallery
            {
                // GalleryData.Id and GallerData.Name can't be mocked so will be null here in tests
                Id = armOperation.Value.Data.Id?.ToString() ?? string.Empty,
                Location = armOperation.Value.Data.Location,
                Name = armOperation.Value.Data.Name
            };
            return gallery;
        }
    }
}
