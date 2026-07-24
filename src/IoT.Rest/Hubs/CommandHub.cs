// IoT.Rest/Hubs/CommandHub.cs
using IoT.Domain.Enums;
using IoT.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Rest.Hubs;

public class CommandHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;

    public CommandHub(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public override async Task OnConnectedAsync()
    {
        var deviceIdStr = Context.GetHttpContext()?.Request.Query["deviceId"];

        if (Guid.TryParse(deviceIdStr, out var deviceId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId.ToString());
            
            var pendingCommands = await _unitOfWork.DeviceCommands
                .GetPendingByDeviceIdAsync(deviceId);

            foreach (var command in pendingCommands)
            {
                command.Status = CommandStatus.Sent;
                command.SentAt = DateTime.UtcNow;
                await _unitOfWork.DeviceCommands.UpdateAsync(command);
                await Clients.Caller.SendAsync("ReceiveCommand", command);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        await base.OnConnectedAsync();
    }

    public async Task AcknowledgeCommand(Guid commandId)
    {
        var command = await _unitOfWork.DeviceCommands.GetByIdAsync(commandId);
        if (command == null) return;

        command.Status = CommandStatus.Acknowledged;
        command.AcknowledgedAt = DateTime.UtcNow;
        await _unitOfWork.DeviceCommands.UpdateAsync(command);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteCommand(Guid commandId, bool success)
    {
        var command = await _unitOfWork.DeviceCommands.GetByIdAsync(commandId);
        if (command == null) return;

        command.Status = success ? CommandStatus.Success : CommandStatus.Failed;
        await _unitOfWork.DeviceCommands.UpdateAsync(command);
        await _unitOfWork.SaveChangesAsync();
    }
}