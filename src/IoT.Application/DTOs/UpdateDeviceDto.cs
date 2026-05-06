// IoT.Application/Devices/DTOs/UpdateDeviceDto.cs
using IoT.Domain.Enums;

namespace IoT.Application.DTOs;

public record UpdateDeviceDto(
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId
);