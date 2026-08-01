using TelemetryService.Domain.Abstractions;
using TelemetryService.Domain.ValueObjects;

namespace TelemetryService.Domain.Entities;

public class TelemetryRecord : AggregateRoot
{
    public DeviceId DeviceId { get; private set; } = null!;
    public Guid MessageId { get; private set; }
    public string Payload { get; private set; } = string.Empty;
    public DateTime ReceivedAt { get; private set; }

    private TelemetryRecord() { }

    public static TelemetryRecord Create(DeviceId deviceId, Guid messageId, string payload)
    {
        return new TelemetryRecord
        {
            DeviceId = deviceId,
            MessageId = messageId,
            Payload = payload,
            ReceivedAt = DateTime.UtcNow
        };
    }
}