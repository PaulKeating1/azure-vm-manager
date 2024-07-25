using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Mocking;
using Azure.ResourceManager.Compute.Models;
using AzureVmManager.Services.Implementations;
using Elector8EnvironmentManager.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace AzureVmManager.Services.Tests
{
    internal class GetImagesServiceTests
    {
        [Test]
        public void Should_GetImages()
        {
            // Arrange
            const string imageLocation = "Testlocation";
            const string imagePublisher = "Test publisher";
            const string imageOffer = "Test image";
            const string imageSku = "Test SKU";
            const string subscriptionId = "test-subscription";
            const string resourceGroupName = "test-reource-group";
            const string galleryName = "test-gallery";

            var clientFactoryMock = new Mock<IArmClientFactory>();
            var clientMock = new Mock<ArmClient>();
            var clientExtensionMock = new Mock<MockableComputeArmClient>();
            var galleryResourceMock = new Mock<GalleryResource>();
            var galleryImageCollectionMock = new Mock<GalleryImageCollection>();
            var galleryImmageResourceMock = new Mock<GalleryImageResource>();
            var galleryIdentifier = GalleryResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, galleryName);

            galleryImmageResourceMock.Setup(x => x.Data).Returns(new GalleryImageData(new AzureLocation(imageLocation)) { Identifier = new GalleryImageIdentifier(imagePublisher, imageOffer, imageSku) });
            clientExtensionMock.Setup(x => x.GetGalleryResource(It.Is<ResourceIdentifier>(x => x.ToString() == galleryIdentifier))).Returns(galleryResourceMock.Object);
            galleryImageCollectionMock.As<IEnumerable<GalleryImageResource>>().Setup(x => x.GetEnumerator()).Returns(GalleryImageCollectionEnumerator(galleryImmageResourceMock));
            galleryResourceMock.Setup(x => x.GetGalleryImages()).Returns(galleryImageCollectionMock.Object);
            clientMock.Setup(c => c.GetCachedClient(It.IsAny<Func<ArmClient, MockableComputeArmClient>>())).Returns(clientExtensionMock.Object);
            clientFactoryMock.Setup(x => x.CreateClient()).Returns(clientMock.Object);

            var getImageService = new GetImagesService(clientFactoryMock.Object);

            // Act
            var images = getImageService.GetImages(subscriptionId, resourceGroupName, galleryName);

            // Assert
            images.Count().Should().Be(1);
            images.First().Location.Should().Be(imageLocation);
            images.First().Publisher.Should().Be(imagePublisher);
            images.First().Offer.Should().Be(imageOffer);
            images.First().Sku.Should().Be(imageSku);
        }

        private IEnumerator<GalleryImageResource> GalleryImageCollectionEnumerator(Mock<GalleryImageResource> galleryImmageResourceMock)
        {
            yield return galleryImmageResourceMock.Object;
        }
    }
}
