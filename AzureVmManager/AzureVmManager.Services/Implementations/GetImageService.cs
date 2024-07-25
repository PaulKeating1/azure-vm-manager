using Azure.ResourceManager.Compute;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using Elector8EnvironmentManager.Services.Interfaces;

namespace AzureVmManager.Services.Implementations
{
    public class GetImageService : IGetImageService
    {
        private readonly IArmClientFactory _armClientFactory;

        public GetImageService(
            IArmClientFactory armClientFactory)
        {
            _armClientFactory = armClientFactory;
        }

        public IEnumerable<Image> GetImages(
            string subscriptionId, 
            string resourceGroupName, 
            string galleryName)
        {
            var client = _armClientFactory.CreateClient();
            var galleryResourceId = GalleryResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, galleryName);
            var gallery = client.GetGalleryResource(galleryResourceId);
            var collection = gallery.GetGalleryImages();
            var images = collection.Select(x => new Image
            {
                Name = x.Data.Name,
                Location = x.Data.Location.Name,
                Publisher = x.Data.Identifier.Publisher,
                Offer = x.Data.Identifier.Offer,
                Sku = x.Data.Identifier.Sku
            }).ToList();
            return images;
        }
    }
}
