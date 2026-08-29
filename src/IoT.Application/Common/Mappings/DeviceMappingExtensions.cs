using IoT.Contracts.Devices;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class DeviceMappingExtensions
{
    

    public static DeviceResponse ToResponse(this Device device)
        => new(
            device.Id,
            device.Name,
            device.Type,
            device.AdminStatus,
            device.LastSeen,
            device.Manufacturer?.Name,
            device.CreatedAt);
}
