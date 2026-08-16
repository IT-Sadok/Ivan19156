using IoT.Contracts.DeviceCommands;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceCommands;

public record GetDeviceCommandsQuery(Guid DeviceId) : IRequest<Result<IEnumerable<DeviceCommandResponse>>>;
