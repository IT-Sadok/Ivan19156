using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace TelemetryService.Application.Commands.ProcessTelemetry;

public record ProcessTelemetryCommand(
    Guid DeviceId,
    Guid MessageId,
    string Payload) : IRequest<Result<bool>>;