using Azure.ResourceManager;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using AzureVmManager.Services.Implementations;
using Elector8EnvironmentManager.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace AzureVmManager.Services.Tests
{
    internal class GetSubscriptionsServiceTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(2)]
        public void Should_GetSubscriptions(int numberOfSubscriptions)
        {
            // Arrange
            var testSubscriptions = Enumerable.Range(0, numberOfSubscriptions).Select(x =>
            {
                
                var mockSubscriptionResource = new Mock<SubscriptionResource>();                
                mockSubscriptionResource.Setup(x => x.Data).Returns(ResourceManagerModelFactory.SubscriptionData(subscriptionId: Guid.NewGuid().ToString(), displayName: "Test subscription"));
                return mockSubscriptionResource;
            }).ToList();
            var clientFactoryMock = new Mock<IArmClientFactory>();
            var clientMock = new Mock<ArmClient>();
            var subscriptionCollection = new Mock<SubscriptionCollection>();            
            subscriptionCollection.As<IEnumerable<SubscriptionResource>>().Setup(x => x.GetEnumerator())
                .Returns(SubscriptionCollectionEnumerator(testSubscriptions.FirstOrDefault(), testSubscriptions.LastOrDefault()));
            clientMock.Setup(x => x.GetSubscriptions()).Returns(subscriptionCollection.Object);
            clientFactoryMock.Setup(x => x.CreateClient()).Returns(clientMock.Object);

            var getSubscriptionsService = new GetSubscriptionsService(clientFactoryMock.Object);
            
            // Act
            var subscriptions = getSubscriptionsService.GetSubscriptions();

            // Assert
            subscriptions.Should().NotBeNull();
            subscriptions.Count().Should().Be(numberOfSubscriptions);
        }

        private IEnumerator<SubscriptionResource> SubscriptionCollectionEnumerator(
            Mock<SubscriptionResource>? subscriptionResourceMock1, 
            Mock<SubscriptionResource>? subscriptionResourceMock2)
        {
            if (subscriptionResourceMock1 != null)
                yield return subscriptionResourceMock1.Object;

            if (subscriptionResourceMock2 != null)
                yield return subscriptionResourceMock2.Object;
        }
    }
}
