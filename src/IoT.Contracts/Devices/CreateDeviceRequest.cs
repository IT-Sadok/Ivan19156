using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record CreateDeviceRequest(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId
);
