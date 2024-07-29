using Azure.ResourceManager.Resources;
using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using Moq;

namespace AzureVmManager.WebApi.Tests
{
    internal class ResourceGroupsControllerTests
    {
        private ResourceGroupsController CreateResourceGroupsController(
            IGetResourceGroupsService getResourceGroupsService = null,
            ICreateResourceGroupService createResourceGroupService = null)
        {
            return new ResourceGroupsController(getResourceGroupsService, createResourceGroupService);
        }

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
            var resourceGroupsController = CreateResourceGroupsController(getResourceGroupsService: getResourceGroupsServiceMock.Object);

            // Act
            var resourceGroups = resourceGroupsController.Get(subscriptionId);

            // Assert
            resourceGroups.Should().NotBeNull();
            resourceGroups.Count().Should().Be(numberOfResourceGroups);
        }

        [Test]
        public async Task Should_Create()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string location = "uksouth";
            const string resourceGroupName = "test-resource-group";

            var createResourceGroupServiceMock = new Mock<ICreateResourceGroupService>();
            createResourceGroupServiceMock.Setup(x => x.Create(subscriptionId, location, resourceGroupName))
                .ReturnsAsync(new ResourceGroup
                {
                    Id = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName).ToString(),
                    Location = location,
                    Name = resourceGroupName
                });

            var resourceGroupsController = CreateResourceGroupsController(createResourceGroupService: createResourceGroupServiceMock.Object);

            // Act
            var resourceGroup = await resourceGroupsController.Create(subscriptionId, location, resourceGroupName);

            // Assert
            resourceGroup.Should().NotBeNull();
            resourceGroup.Name.Should().Be(resourceGroupName);
            resourceGroup.Location.Should().Be(location);
        }
    }
}
