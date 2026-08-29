using IoT.Contracts.DeviceCommands;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class DeviceCommandMappingExtensions
{
    

    public static DeviceCommandResponse ToResponse(this DeviceCommand command)
        => new(
            command.Id,
            command.DeviceId,
            command.CommandType.Slug,
            command.Parameters,
            command.Priority,
            command.Status,
            command.CreatedAt,
            command.SentAt,
            command.AcknowledgedAt,
            command.ExpiresAt);
}
