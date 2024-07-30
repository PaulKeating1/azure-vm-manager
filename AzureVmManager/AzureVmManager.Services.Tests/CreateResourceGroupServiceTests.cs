using Azure;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Implementations;
using AzureVmManager.Services.Interfaces;
using FluentAssertions;
using Moq;
using Azure.ResourceManager.Models;

namespace AzureVmManager.Services.Tests
{
    internal class CreateResourceGroupServiceTests
    {
        [Test]
        public async Task Should_Create()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string location = "uksouth";
            const string resourceGroupName = "test-resource-group";

            var subscriptionResourceMock = new Mock<SubscriptionResource>();
            var getSubscriptionResourceServiceMock = new Mock<IGetSubscriptionResourceService>();
            getSubscriptionResourceServiceMock.Setup(x => x.GetSubscriptionResource(subscriptionId)).Returns(subscriptionResourceMock.Object);

            var resourceGroupCollectionMock = new Mock<ResourceGroupCollection>();
            subscriptionResourceMock.Setup(x => x.GetResourceGroups()).Returns(resourceGroupCollectionMock.Object);

            var armOperationMock = new Mock<ArmOperation<ResourceGroupResource>>();
            var resourceGroupResourceMock = new Mock<ResourceGroupResource>();
            var resourceGroupData = ResourceManagerModelFactory.ResourceGroupData(id: ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName), name: resourceGroupName, location: location);
            resourceGroupResourceMock.Setup(x => x.Data).Returns(resourceGroupData);
            armOperationMock.Setup(x => x.Value).Returns(resourceGroupResourceMock.Object);

            resourceGroupCollectionMock.Setup(x => 
                    x.CreateOrUpdateAsync(
                        WaitUntil.Completed, 
                        resourceGroupName, 
                        It.Is<ResourceGroupData>(x => x.Location == location), 
                        CancellationToken.None))
                .ReturnsAsync(armOperationMock.Object);

            var createResourceGroupService = new CreateResourceGroupService(getSubscriptionResourceServiceMock.Object);

            // Act
            var resourceGroup = await createResourceGroupService.Create(subscriptionId, location, resourceGroupName);

            // Assert
            resourceGroup.Should().NotBeNull();
            resourceGroup.Name.Should().Be(resourceGroupName);
            resourceGroup.Location.Should().Be(location);
        }
    }
}
