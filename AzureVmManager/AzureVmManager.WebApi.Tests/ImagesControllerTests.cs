using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using Moq;

namespace AzureVmManager.WebApi.Tests
{
    internal class ImagesControllerTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(2)]
        public void Should_GetImages(int numberOfImages)
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string resourceGroupName = "test-resource-group";
            const string galleryName = "test-gallery";
            var imageCollection = Enumerable.Range(0, numberOfImages).Select(x => new Image { Location = "uksouth", Name = "", Offer = "", Publisher = "", Sku = "" });
            var getImagesServiceMock = new Mock<IGetImagesService>();
            getImagesServiceMock.Setup(x => x.GetImages(subscriptionId, resourceGroupName, galleryName)).Returns(imageCollection);
            var imagesController = new ImagesController(getImagesServiceMock.Object);

            // Act
            var images = imagesController.Get(subscriptionId, resourceGroupName, galleryName);

            // Assert
            images.Should().NotBeNull();
            images.Count().Should().Be(numberOfImages);
        }
    }
}
