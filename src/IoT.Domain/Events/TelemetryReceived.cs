namespace IoT.Domain.Events;

public record TelemetryReceivedEvent(
    Guid DeviceId,
    Guid MessageId,
    string Payload,
    DateTime ReceivedAt
);