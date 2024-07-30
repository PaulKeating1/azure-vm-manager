using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Implementations;
using Elector8EnvironmentManager.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace AzureVmManager.Services.Tests
{
    internal class GetResourceGroupResourceServiceTests
    {
        [Test]
        public async Task Should_Get()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string resourceGroupName = "test-resource-group";
            var resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);

            var clientFactoryMock = new Mock<IArmClientFactory>();
            var clientMock = new Mock<ArmClient>();

            var resourceGroupResourceMock = new Mock<ResourceGroupResource>();
            clientMock.Setup(x => x.GetResourceGroupResource(It.Is<ResourceIdentifier>(x => x.ToString() == resourceGroupResourceId))).Returns(resourceGroupResourceMock.Object);

            var resourceGroupResourceWithDataMock = new Mock<ResourceGroupResource>();
            var resourceGroupData = ResourceManagerModelFactory.ResourceGroupData(id: resourceGroupResourceId, name: resourceGroupName);
            resourceGroupResourceWithDataMock.Setup(x => x.Data).Returns(resourceGroupData);
            var response = new Mock<Response<ResourceGroupResource>>();
            response.SetupGet(x => x.Value).Returns(resourceGroupResourceWithDataMock.Object);
            resourceGroupResourceMock.Setup(x => x.GetAsync(default)).ReturnsAsync(response.Object);

            clientFactoryMock.Setup(x => x.CreateClient()).Returns(clientMock.Object);

            var getResourceGroupResourceService = new GetResourceGroupResourceService(clientFactoryMock.Object);

            // Act
            var resourceGroupResource = await getResourceGroupResourceService.Get(resourceGroupResourceId.ToString());

            // Assert
            resourceGroupResource.Should().NotBeNull();
            resourceGroupResource.Data.Id.Should().Be(resourceGroupResourceId);
            resourceGroupResource.Data.Name.Should().Be(resourceGroupName);
        }
    }
}
