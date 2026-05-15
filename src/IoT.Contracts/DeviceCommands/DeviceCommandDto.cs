using IoT.Domain.Enums;

namespace IoT.Contracts.DeviceCommands;

public record DeviceCommandResponse(
    Guid Id,
    Guid DeviceId,
    string CommandTypeSlug,
    string? Parameters,
    int Priority,
    CommandStatus Status,
    DateTime CreatedAt,
    DateTime? SentAt,
    DateTime? AcknowledgedAt,
    DateTime? ExpiresAt
);
