using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record DeviceDto(
    Guid Id,
    string Name,
    DeviceType Type,
    DeviceAdminStatus AdminStatus,
    DateTime? LastSeen,
    string? ManufacturerName,
    DateTime CreatedAt
);
