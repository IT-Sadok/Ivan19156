using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class TelemetryRecord : AggregateRoot
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid MessageId { get; set; }
    public string Payload { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}