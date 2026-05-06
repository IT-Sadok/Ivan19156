using IoT.Domain.Enums;

namespace IoT.Application.DTOs;

public record CreateDeviceDto(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId
);