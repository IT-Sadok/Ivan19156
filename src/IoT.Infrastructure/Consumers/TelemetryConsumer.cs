using IoT.Domain.Entities;
using IoT.Domain.Events;
using IoT.Interfaces;
using MassTransit;

namespace IoT.Infrastructure.Consumers;

public class TelemetryConsumer : IConsumer<TelemetryReceivedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public TelemetryConsumer(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task Consume(ConsumeContext<TelemetryReceivedEvent> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;

        // Idempotency check
        var exists = await _unitOfWork.Telemetry
            .ExistsAsync(message.DeviceId, message.MessageId, ct);

        if (exists)
            return;

        // Save telemetry record
        var record = new TelemetryRecord
        {
            Id = Guid.NewGuid(),
            DeviceId = message.DeviceId,
            MessageId = message.MessageId,
            Payload = message.Payload,
            ReceivedAt = message.ReceivedAt
        };

        await _unitOfWork.Telemetry.AddAsync(record, ct);

        // Update device LastSeen
        var device = await _unitOfWork.Devices.GetByIdAsync(message.DeviceId, ct);
        if (device != null)
        {
            device.LastSeen = message.ReceivedAt;
            await _unitOfWork.Devices.UpdateAsync(device);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
