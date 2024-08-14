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
    internal class GetSubscriptionResourceServiceTests
    {
        [Test]
        public void Should_GetSubscriptionResource()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            const string displayName = "Test subscription";

            var subscriptionResourceWithDataMock = new Mock<SubscriptionResource>();
            var subscriptionData = ResourceManagerModelFactory.SubscriptionData(subscriptionId: subscriptionId, displayName: displayName);
            subscriptionResourceWithDataMock.Setup(x => x.Data).Returns(subscriptionData);
            var response = new Mock<Response<SubscriptionResource>>();
            response.SetupGet(x => x.Value).Returns(subscriptionResourceWithDataMock.Object);
            var subscriptionResourceMock = new Mock<SubscriptionResource>();
            subscriptionResourceMock.Setup(x => x.Get(default)).Returns(response.Object);

            var clientFactoryMock = new Mock<IArmClientFactory>();
            var clientMock = new Mock<ArmClient>();
            var resourceIdentifier = SubscriptionResource.CreateResourceIdentifier(subscriptionId);
            clientMock.Setup(x => x.GetSubscriptionResource(It.Is<ResourceIdentifier>(x => x.ToString() == resourceIdentifier.ToString())))
                .Returns(subscriptionResourceMock.Object);
            clientFactoryMock.Setup(x => x.CreateClient()).Returns(clientMock.Object);

            var getSubscriptionResourceService = new GetSubscriptionResourceService(clientFactoryMock.Object);

            // Act
            var subscriptionResource = getSubscriptionResourceService.GetSubscriptionResource(subscriptionId);

            // Assert
            subscriptionResource.Should().NotBeNull();
            subscriptionResource.Data.Should().NotBeNull();
            subscriptionResource.Data.SubscriptionId.Should().Be(subscriptionId);
            subscriptionResource.Data.DisplayName.Should().Be(displayName);
        }
    }
}
