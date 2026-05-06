using IoT.Contracts.DeviceCommands;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.DeviceCommands.GetDeviceCommands;

public record GetDeviceCommandsQuery(Guid DeviceId)
    : IRequest<Result<IEnumerable<DeviceCommandDto>>>;
