using DeviceService.Domain.Entities;
using IoT.Contracts.Devices;
using IoT.Domain.Enums;

namespace DeviceService.Application.Common.Mappings;

public static class DeviceMappings
{
    public static DeviceResponse ToDto(this Device device) => new(
        device.Id,
        device.Name,
        MapDeviceType(device.Type),
        MapAdminStatus(device.AdminStatus),
        device.LastSeen,
        device.Manufacturer?.Name,
        device.CreatedAt);

    private static DeviceType MapDeviceType(DeviceService.Domain.Enums.DeviceType type) => type switch
    {
        DeviceService.Domain.Enums.DeviceType.Sensor => DeviceType.Sensor,
        DeviceService.Domain.Enums.DeviceType.Actuator => DeviceType.Actuator,
        _ => DeviceType.Sensor
    };

    private static DeviceAdminStatus MapAdminStatus(DeviceService.Domain.Enums.DeviceAdminStatus status) => status switch
    {
        DeviceService.Domain.Enums.DeviceAdminStatus.Active => DeviceAdminStatus.Active,
        DeviceService.Domain.Enums.DeviceAdminStatus.Disabled => DeviceAdminStatus.Disabled,
        DeviceService.Domain.Enums.DeviceAdminStatus.Decommissioned => DeviceAdminStatus.Decommissioned,
        _ => DeviceAdminStatus.Active
    };
}
