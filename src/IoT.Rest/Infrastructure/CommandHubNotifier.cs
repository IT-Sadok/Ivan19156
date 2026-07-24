using IoT.Interfaces.Services;
using IoT.Rest.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Rest.Infrastructure;

public class CommandHubNotifier : ICommandHubNotifier
{
    private readonly IHubContext<CommandHub> _hubContext;

    public CommandHubNotifier(IHubContext<CommandHub> hubContext)
        => _hubContext = hubContext;

    public Task NotifyCommandAsync(Guid deviceId, object command)
        => _hubContext.Clients
            .Group(deviceId.ToString())
            .SendAsync("ReceiveCommand", command);
}
