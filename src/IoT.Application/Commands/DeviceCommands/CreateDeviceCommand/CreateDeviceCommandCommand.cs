using IoT.Contracts.DeviceCommands;
using IoT.Domain.Enums;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;

public record CreateDeviceCommandCommand(
    Guid DeviceId,
    CommandTypeSlug CommandTypeSlug,
    string? Parameters,
    int Priority = 0,
    DateTime? ExpiresAt = null) : IRequest<Result<DeviceCommandResponse>>;
