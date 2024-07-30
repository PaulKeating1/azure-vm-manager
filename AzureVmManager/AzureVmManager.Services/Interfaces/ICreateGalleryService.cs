using AzureVmManager.DataObjects;

namespace AzureVmManager.Services.Interfaces
{
    public interface ICreateGalleryService
    {
        Task<Gallery> Create(string resourceGroupResourceId, string galleryName);
    }
}
