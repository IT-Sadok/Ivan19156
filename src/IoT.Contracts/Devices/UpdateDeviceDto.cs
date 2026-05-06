using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record UpdateDeviceDto(
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId
);
