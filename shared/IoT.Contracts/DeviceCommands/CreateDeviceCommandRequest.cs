using IoT.Domain.Enums;

namespace IoT.Contracts.DeviceCommands;

public record CreateDeviceCommandRequest(
    CommandTypeSlug CommandTypeSlug,
    string? Parameters,
    int Priority = 0,
    DateTime? ExpiresAt = null
);
