using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.UpdateDevice;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId) : IRequest<Result<DeviceResponse>>;
