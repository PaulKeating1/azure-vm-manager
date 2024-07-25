using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetImagesService
    {
        IEnumerable<Image> GetImages(
            string subscriptionId,
            string resourceGroupName,
            string galleryName);
    }
}
