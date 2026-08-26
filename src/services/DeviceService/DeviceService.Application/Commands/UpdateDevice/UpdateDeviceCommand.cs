using DeviceService.Domain.Enums;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.UpdateDevice;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId) : IRequest<Result<bool>>;
