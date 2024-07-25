using Azure.ResourceManager;

namespace Elector8EnvironmentManager.Services.Interfaces
{
    public interface IArmClientFactory
    {
        ArmClient CreateClient();
    }
}
