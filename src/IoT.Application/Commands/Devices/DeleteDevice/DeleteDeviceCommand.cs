using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.DeleteDevice;

public record DeleteDeviceCommand(Guid Id) : IRequest<Result<bool>>;
