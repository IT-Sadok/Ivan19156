using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record DeviceResponse(
    Guid Id,
    string Name,
    DeviceType Type,
    DeviceAdminStatus AdminStatus,
    DateTime? LastSeen,
    string? ManufacturerName,
    DateTime CreatedAt
);
