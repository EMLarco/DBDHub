using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace DBDHub.Hubs;

public class LiveHub : Hub
{
    private static readonly ConcurrentDictionary<string, byte> Connections = new();
    private static readonly string ServerName =
        Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName;

    public override async Task OnConnectedAsync()
    {
        Connections.TryAdd(Context.ConnectionId, 0);
        await Clients.Caller.SendAsync("connected", new
        {
            connectionId = Context.ConnectionId,
            server = ServerName
        });
        await BroadcastClientsCountAsync();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Connections.TryRemove(Context.ConnectionId, out _);
        await BroadcastClientsCountAsync();
        await base.OnDisconnectedAsync(exception);
    }

    public Task Ping(string message)
    {
        return Clients.All.SendAsync("liveEvent", new
        {
            type = "ping",
            message,
            data = (object?)null,
            at = DateTime.UtcNow,
            server = ServerName
        });
    }

    private Task BroadcastClientsCountAsync()
    {
        return Clients.All.SendAsync("clientsOnline", new
        {
            server = ServerName,
            count = Connections.Count
        });
    }
}
