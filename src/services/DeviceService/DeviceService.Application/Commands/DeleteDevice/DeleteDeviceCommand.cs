using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.DeleteDevice;

public record DeleteDeviceCommand(Guid Id) : IRequest<Result<bool>>;
