namespace IoT.Domain.Events;

public record TelemetryReceived(
    Guid DeviceId,
    Guid MessageId,
    string Payload,
    DateTime ReceivedAt
);