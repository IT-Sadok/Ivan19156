using DeviceService.Domain.Entities;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Enums;

namespace DeviceService.Application.Common.Mappings;

public static class CommandMappings
{
    public static DeviceCommandResponse ToDto(this DeviceCommand command) => new(
        command.Id,
        command.DeviceId,
        command.CommandType?.Slug ?? string.Empty,
        command.Payload,
        0,
        MapCommandStatus(command.Status),
        command.CreatedAt,
        command.SentAt,
        command.AcknowledgedAt,
        null);

    private static CommandStatus MapCommandStatus(DeviceService.Domain.Enums.CommandStatus status) => status switch
    {
        DeviceService.Domain.Enums.CommandStatus.Created => CommandStatus.Created,
        DeviceService.Domain.Enums.CommandStatus.Sent => CommandStatus.Sent,
        DeviceService.Domain.Enums.CommandStatus.Acknowledged => CommandStatus.Acknowledged,
        DeviceService.Domain.Enums.CommandStatus.Failed => CommandStatus.Failed,
        _ => CommandStatus.Created
    };
}
