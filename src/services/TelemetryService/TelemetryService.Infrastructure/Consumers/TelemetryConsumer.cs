using IoT.Contracts.Telemetry;
using MassTransit;
using TelemetryService.Domain.Entities;
using TelemetryService.Domain.Events;
using TelemetryService.Domain.ValueObjects;
using TelemetryService.Interfaces;

namespace TelemetryService.Infrastructure.Consumers;

public class TelemetryConsumer : IConsumer<TelemetryReceivedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public TelemetryConsumer(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<TelemetryReceivedEvent> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;

        var exists = await _unitOfWork.Telemetry
            .ExistsAsync(message.DeviceId.Value, message.MessageId, ct);

        if (exists)
            return;

        var record = TelemetryRecord.Create(
            message.DeviceId,
            message.MessageId,
            message.Payload);

        await _unitOfWork.Telemetry.AddAsync(record, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        await _publishEndpoint.Publish(new TelemetryStoredEvent(
            message.DeviceId.Value,
            message.MessageId,
            message.ReceivedAt), ct);
    }
}