using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Services;

namespace IoT.Infrastructure.Services;

public class CommandNotificationService : ICommandNotificationService
{
    private readonly ICommandHubNotifier _hubNotifier;
    private readonly IUnitOfWork _unitOfWork;

    public CommandNotificationService(
        ICommandHubNotifier hubNotifier,
        IUnitOfWork unitOfWork)
    {
        _hubNotifier = hubNotifier;
        _unitOfWork = unitOfWork;
    }

    public async Task SendCommandToDeviceAsync(Guid deviceId, DeviceCommand command)
    {
        try
        {
            await _hubNotifier.NotifyCommandAsync(deviceId, command);

            command.Status = CommandStatus.Sent;
            command.SentAt = DateTime.UtcNow;
            await _unitOfWork.DeviceCommands.UpdateAsync(command);
            await _unitOfWork.SaveChangesAsync();
        }
        catch
        {
            // Device offline — command remains with Created status
        }
    }
}
