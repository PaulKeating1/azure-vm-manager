using AzureVmManager.Services.Implementations;
using FluentAssertions;

namespace AzureVmManager.Services.Tests
{
    internal class ArmClientFactoryTests
    {
        [Test]
        public void Should_CreateClient()
        {
            // Arrange
            var armClientFactory = new ArmClientFactory();

            // Act
            var client = armClientFactory.CreateClient();

            // Assert
            client.Should().NotBeNull();
        }
    }
}
