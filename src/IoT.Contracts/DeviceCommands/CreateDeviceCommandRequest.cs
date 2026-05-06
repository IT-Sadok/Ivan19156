namespace IoT.Contracts.DeviceCommands;

public record CreateDeviceCommandRequest(
    string CommandTypeSlug,
    string? Parameters,
    int Priority = 0,
    DateTime? ExpiresAt = null
);
