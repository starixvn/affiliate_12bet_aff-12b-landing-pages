using EasyNetQ;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface IRabbitBus
{
    void SetBus(IAdvancedBus bus);
    Task<bool> PublishAsync<T>(T model);
    Task RegisterQueueAsync();
    bool IsConnected { get; }
    Task<Dictionary<string, object>> GetConnectionInfoAsync();
}
