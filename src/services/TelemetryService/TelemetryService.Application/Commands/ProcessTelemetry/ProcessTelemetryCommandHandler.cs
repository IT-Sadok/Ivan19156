using IoT.Shared.Common;
using IoT.Shared.Mediator;
using MassTransit;
using TelemetryService.Domain.Events;
using TelemetryService.Domain.ValueObjects;

namespace TelemetryService.Application.Commands.ProcessTelemetry;

public class ProcessTelemetryCommandHandler
    : IRequestHandler<ProcessTelemetryCommand, Result<bool>>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessTelemetryCommandHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task<Result<bool>> ExecuteAsync(
        ProcessTelemetryCommand request,
        CancellationToken ct = default)
    {
        await _publishEndpoint.Publish(new TelemetryReceivedEvent(
            DeviceId.From(request.DeviceId),
            request.MessageId,
            request.Payload,
            DateTime.UtcNow), ct);

        return Result<bool>.Success(true);
    }
}