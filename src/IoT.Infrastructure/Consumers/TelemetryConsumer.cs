// IoT.Infrastructure/Consumers/TelemetryConsumer.cs
using IoT.Domain.Entities;
using IoT.Domain.Events;
using IoT.Interfaces;
using MassTransit;

namespace IoT.Infrastructure.Consumers;

public class TelemetryConsumer : IConsumer<TelemetryReceived>
{
    private readonly IUnitOfWork _unitOfWork;

    public TelemetryConsumer(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task Consume(ConsumeContext<TelemetryReceived> context)
    {
        var message = context.Message;

        // Idempotency check
        var exists = await _unitOfWork.Telemetry
            .ExistsAsync(message.DeviceId, message.MessageId);

        if (exists)
            return;

        // Зберегти телеметрію
        var record = new TelemetryRecord
        {
            Id = Guid.NewGuid(),
            DeviceId = message.DeviceId,
            MessageId = message.MessageId,
            Payload = message.Payload,
            ReceivedAt = message.ReceivedAt
        };

        await _unitOfWork.Telemetry.AddAsync(record);

        // Оновити LastSeen девайса
        var device = await _unitOfWork.Devices.GetByIdAsync(message.DeviceId);
        if (device != null)
        {
            device.LastSeen = message.ReceivedAt;
            await _unitOfWork.Devices.UpdateAsync(device);
        }

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}