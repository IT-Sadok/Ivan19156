// IoT.Application/Devices/DTOs/DeviceDto.cs
using IoT.Domain.Enums;

namespace IoT.Application.DTOs;

public record DeviceDto(
    Guid Id,
    string Name,
    DeviceType Type,
    DeviceAdminStatus AdminStatus,
    DateTime? LastSeen,
    string? ManufacturerName,
    DateTime CreatedAt
);