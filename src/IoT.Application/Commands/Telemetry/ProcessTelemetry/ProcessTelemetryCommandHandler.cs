using IoT.Domain.Events;
using IoT.Shared.Mediator;
using IoT.Shared.Common;
using MassTransit;

namespace IoT.Application.Commands.Telemetry.ProcessTelemetry;

public class ProcessTelemetryCommandHandler
    : IRequestHandler<ProcessTelemetryCommand, Result<bool>>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessTelemetryCommandHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task<Result<bool>> Handle(
        ProcessTelemetryCommand request,
        CancellationToken ct = default)
    {
        await _publishEndpoint.Publish(new TelemetryReceivedEvent(
            request.DeviceId,
            request.MessageId,
            request.Payload,
            DateTime.UtcNow), ct);

        return Result<bool>.Success(true);
    }
}
