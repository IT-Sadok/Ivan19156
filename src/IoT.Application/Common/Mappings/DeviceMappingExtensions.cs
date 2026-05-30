using IoT.Application.Commands.Devices.CreateDevice;
using IoT.Application.Commands.Devices.UpdateDevice;
using IoT.Contracts.Devices;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class DeviceMappingExtensions
{
    public static CreateDeviceCommand ToCommand(this CreateDeviceRequest request)
        => new(request.Name, request.Type, request.ManufacturerId);

    public static UpdateDeviceCommand ToCommand(this UpdateDeviceRequest request)
        => new(Guid.Empty, request.Name, request.AdminStatus, request.ManufacturerId);

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
