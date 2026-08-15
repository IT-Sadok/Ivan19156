using IoT.Contracts.DeviceCommands;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.DeviceCommands.GetDeviceCommands;

public record GetDeviceCommandsQuery(Guid DeviceId)
    : IRequest<Result<IEnumerable<DeviceCommandResponse>>>;
