using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Mocking;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Implementations;
using AzureVmManager.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace AzureVmManager.Services.Tests
{
    internal class CreateGalleryServiceTests
    {
        [Test]
        public async Task Should_Create()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string location = "uksouth";
            const string resourceGroupName = "rg-test";
            var resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);
            const string galleryName = "shg-test";

            var resourceGroupResourceMock = new Mock<ResourceGroupResource>();
            resourceGroupResourceMock.Setup(x => x.Data).Returns(ResourceManagerModelFactory.ResourceGroupData(location: location));
            var resourceGroupResourceExtensionMock = new Mock<MockableComputeResourceGroupResource>();            

            resourceGroupResourceMock.Setup(rg => rg.GetCachedClient(It.IsAny<Func<ArmClient, MockableComputeResourceGroupResource>>())).Returns(resourceGroupResourceExtensionMock.Object);
            var getResourceGroupResourceServiceMock = new Mock<IGetResourceGroupResourceService>();
            getResourceGroupResourceServiceMock.Setup(x => x.Get(resourceGroupResourceId.ToString())).ReturnsAsync(resourceGroupResourceMock.Object);

            var galleryCollectionMock = new Mock<GalleryCollection>();
            var armOperationMock = new Mock<ArmOperation<GalleryResource>>();
            var galleryResourceMock = new Mock<GalleryResource>();
            var galleryResourceData = new GalleryData(location);
            galleryResourceMock.Setup(x => x.Data).Returns(galleryResourceData);
            armOperationMock.Setup(x => x.Value).Returns(galleryResourceMock.Object);

            galleryCollectionMock.Setup(x =>
                    x.CreateOrUpdateAsync(
                        WaitUntil.Completed,
                        galleryName,
                        It.Is<GalleryData>(x => x.Location == location), 
                        CancellationToken.None))
                .ReturnsAsync(armOperationMock.Object);

            resourceGroupResourceExtensionMock.Setup(x => x.GetGalleries()).Returns(galleryCollectionMock.Object);

            var createGalleryService = new CreateGalleryService(getResourceGroupResourceServiceMock.Object);

            // Act
            var gallery = await createGalleryService.Create(resourceGroupResourceId.ToString(), galleryName);

            // Assert
            gallery.Should().NotBeNull();
            gallery.Location.Should().Be(location);
        }
    }
}
