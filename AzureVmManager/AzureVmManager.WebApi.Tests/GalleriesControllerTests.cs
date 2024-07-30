using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Resources;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using Moq;

namespace AzureVmManager.WebApi.Tests
{
    internal class GalleriesControllerTests
    {
        [Test]
        public async Task Should_Create()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string location = "uksouth";
            const string resourceGroupName = "rg-test";
            const string galleryName = "shg-test";
            var resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);
            var galleryResourceId = GalleryResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, galleryName);

            var createGalleryService = new Mock<ICreateGalleryService>();
            createGalleryService.Setup(x => x.Create(resourceGroupResourceId.ToString(), galleryName)).ReturnsAsync(new Gallery
            {
                Id = galleryResourceId.ToString(),
                Location = location,
                Name = galleryName
            });

            var galleriesController = new GalleriesController(createGalleryService.Object);

            // Act
            var gallery = await galleriesController.Create(subscriptionId, resourceGroupName, galleryName);

            // Assert
            gallery.Should().NotBeNull();
            gallery.Id.Should().Be(galleryResourceId.ToString());
            gallery.Location.Should().Be(location);
            gallery.Name.Should().Be(galleryName);
        }
    }
}
