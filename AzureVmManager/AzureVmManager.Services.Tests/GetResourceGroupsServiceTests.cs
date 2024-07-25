using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using AzureVmManager.Services.Implementations;
using Elector8EnvironmentManager.Services.Interfaces;
using Moq;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.DataObjects;
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
            var testResourceGroups = Enumerable.Range(0, numberOfResourceGroups).Select(x =>
            {

                var mockResourceGroupResource = new Mock<ResourceGroupResource>();
                var resourceIdentifier = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, "test-resource-group");
                mockResourceGroupResource.Setup(x => x.Data).Returns(ResourceManagerModelFactory.ResourceGroupData(resourceIdentifier));
                return mockResourceGroupResource;
            }).ToList();

            var subscriptionResourceMock = new Mock<SubscriptionResource>();
            subscriptionResourceMock.Setup(x => x.Data).Returns(ResourceManagerModelFactory.SubscriptionData(subscriptionId: subscriptionId, displayName: "Test subscription"));
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
