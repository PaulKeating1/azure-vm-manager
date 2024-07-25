using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using Moq;

namespace AzureVmManager.WebApi.Tests
{
    internal class ResourceGroupsControllerTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(2)]
        public void Should_GetResourceGroups(int numberOfResourceGroups)
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            var resourceGroupCollection = Enumerable.Range(0, numberOfResourceGroups).Select(x => new ResourceGroup { Id = "", Name = "", Location = "" });
            var getResourceGroupsServiceMock = new Mock<IGetResourceGroupsService>();
            getResourceGroupsServiceMock.Setup(x => x.GetResourceGroups(subscriptionId)).Returns(resourceGroupCollection);
            var resourceGroupsController = new ResourceGroupsController(getResourceGroupsServiceMock.Object);

            // Act
            var resourceGroups = resourceGroupsController.Get(subscriptionId);

            // Assert
            resourceGroups.Should().NotBeNull();
            resourceGroups.Count().Should().Be(numberOfResourceGroups);
        }
    }
}
