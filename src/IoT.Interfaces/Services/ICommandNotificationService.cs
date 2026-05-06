using IoT.Domain.Entities;

namespace IoT.Interfaces.Services;

public interface ICommandNotificationService
{
    Task SendCommandToDeviceAsync(Guid deviceId, DeviceCommand command);
}