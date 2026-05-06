// IoT.Application/Devices/Commands/DeleteDevice/DeleteDeviceCommand.cs
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Commands.DeleteDevice;

public record DeleteDeviceCommand(Guid Id) : IRequest<Result<bool>>;