using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record CreateDeviceDto(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId
);
