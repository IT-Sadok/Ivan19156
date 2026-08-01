using IoT.Domain.Enums;

namespace IoT.Contracts.Devices;

public record UpdateDeviceRequest(
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId
);
