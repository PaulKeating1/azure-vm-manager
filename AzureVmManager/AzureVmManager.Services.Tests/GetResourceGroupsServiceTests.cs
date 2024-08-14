using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Implementations;
using Moq;
using AzureVmManager.Services.Interfaces;
using FluentAssertions;

namespace AzureVmManager.Services.Tests
{
    internal class GetResourceGroupsServiceTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(2)]
        public void Should_GetResourceGroups(int numberOfResourceGroups)
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string location = "uksouth";
            var testResourceGroups = Enumerable.Range(0, numberOfResourceGroups).Select(x =>
            {
                var mockResourceGroupResource = new Mock<ResourceGroupResource>();
                var resourceIdentifier = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, $"test-resource-group-{x}");
                mockResourceGroupResource.Setup(x => x.Data).Returns(ResourceManagerModelFactory.ResourceGroupData(resourceIdentifier, $"rg-test={x}", location: location));
                return mockResourceGroupResource;
            }).ToList();

            var subscriptionResourceMock = new Mock<SubscriptionResource>();
            const string subscriptionName = "Test subscription";
            subscriptionResourceMock.Setup(x => x.Data).Returns(ResourceManagerModelFactory.SubscriptionData(subscriptionId: subscriptionId, displayName: subscriptionName));
            var getSubscriptionResourceServiceMock = new Mock<IGetSubscriptionResourceService>();
            getSubscriptionResourceServiceMock.Setup(x => x.GetSubscriptionResource(subscriptionId)).Returns(subscriptionResourceMock.Object);

            var resourceGroupCollection = new Mock<ResourceGroupCollection>();
            resourceGroupCollection.As<IEnumerable<ResourceGroupResource>>().Setup(x => x.GetEnumerator())
                .Returns(ResourceGroupCollectionEnumerator(testResourceGroups.FirstOrDefault(), testResourceGroups.LastOrDefault()));

            subscriptionResourceMock.Setup(x => x.GetResourceGroups()).Returns(resourceGroupCollection.Object);

            var getResourceGroupsService = new GetResourceGroupsService(getSubscriptionResourceServiceMock.Object);

            // Act
            var resourceGroups = getResourceGroupsService.GetResourceGroups(subscriptionId);

            // Assert
            resourceGroups.Should().NotBeNull();
            resourceGroups.Count().Should().Be(numberOfResourceGroups);
            var count = 0;

            foreach (var resourceGroup in resourceGroups)
            {
                var resourceIdentifier = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, $"test-resource-group-{count}");
                resourceGroup.Id.Should().Be(resourceIdentifier);
                resourceGroup.Name.Should().Be($"rg-test={count}");
                resourceGroup.Location.Should().Be(location);
                resourceGroup.SubscriptionName.Should().Be(subscriptionName);
                count++;
            }
        }

        private IEnumerator<ResourceGroupResource> ResourceGroupCollectionEnumerator(
            Mock<ResourceGroupResource>? resourceGroupResourceMock1,
            Mock<ResourceGroupResource>? resourceGroupResourceMock2)
        {
            if (resourceGroupResourceMock1 != null)
                yield return resourceGroupResourceMock1.Object;

            if (resourceGroupResourceMock2 != null)
                yield return resourceGroupResourceMock2.Object;
        }
    }
}
