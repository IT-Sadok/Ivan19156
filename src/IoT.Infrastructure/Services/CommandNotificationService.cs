using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace IoT.Infrastructure.Services;

public class CommandNotificationService : ICommandNotificationService
{
    private readonly ICommandHubNotifier _hubNotifier;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CommandNotificationService> _logger;

    public CommandNotificationService(
        ICommandHubNotifier hubNotifier,
        IUnitOfWork unitOfWork,
        ILogger<CommandNotificationService> logger)
    {
        _hubNotifier = hubNotifier;
        _unitOfWork = unitOfWork;
        _logger = logger;
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
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Device {DeviceId} is offline; command {CommandId} remains in Created status", deviceId, command.Id);
        }
    }
}
