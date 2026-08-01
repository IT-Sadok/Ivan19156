using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Telemetry.ProcessTelemetry;

public record ProcessTelemetryCommand(
    Guid DeviceId,
    Guid MessageId,
    string Payload) : IRequest<Result<bool>>;
