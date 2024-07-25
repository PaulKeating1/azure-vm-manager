using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface IGetImageService
    {
        IEnumerable<Image> GetImages(
            string subscriptionId,
            string resourceGroupName,
            string galleryName);
    }
}
