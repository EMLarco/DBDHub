using DBDHub.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DBDHub.Services;

public interface ILiveEventPublisher
{
    Task PublishAsync(string type, string message, object? data = null);
}

public class LiveEventPublisher : ILiveEventPublisher
{
    private readonly IHubContext<LiveHub> _hubContext;
    private static readonly string ServerName =
        Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName;

    public LiveEventPublisher(IHubContext<LiveHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishAsync(string type, string message, object? data = null)
    {
        return _hubContext.Clients.All.SendAsync("liveEvent", new
        {
            type,
            message,
            data,
            at = DateTime.UtcNow,
            server = ServerName
        });
    }
}
