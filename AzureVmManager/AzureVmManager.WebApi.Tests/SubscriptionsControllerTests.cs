using AzureVmManager.DataObjects;
using AzureVmManager.Services.Interfaces;
using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using Moq;

namespace AzureVmManager.WebApi.Tests
{
    internal class SubscriptionsControllerTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(2)]
        public void Should_GetSubscriptions(int numberOfSubscriptions)
        {
            // Arrange
            var subscriptionCollection = Enumerable.Range(0, numberOfSubscriptions).Select(x => new Subscription { Id = "", Name = "" });
            var getSubscriptionsServiceMock = new Mock<IGetSubscriptionsService>();
            getSubscriptionsServiceMock.Setup(x => x.GetSubscriptions()).Returns(subscriptionCollection);
            var subscriptionsController = new SubscriptionsController(getSubscriptionsServiceMock.Object);

            // Act
            var subscriptions = subscriptionsController.Get();

            // Assert
            subscriptions.Should().NotBeNull();
            subscriptions.Count().Should().Be(numberOfSubscriptions);
        }
    }
}
