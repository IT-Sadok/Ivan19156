using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.AcknowledgeAlert;

public record AcknowledgeAlertCommand(
    Guid AlertId,
    Guid AcknowledgedById,
    string? Note) : IRequest<Result<bool>>;
