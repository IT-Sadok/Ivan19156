using IoT.Contracts.DeviceCommands;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;

public record CreateDeviceCommandCommand(
    Guid DeviceId,
    string CommandTypeSlug,
    string? Parameters,
    int Priority = 0,
    DateTime? ExpiresAt = null) : IRequest<Result<DeviceCommandDto>>;
