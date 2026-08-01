namespace IoT.Contracts.Telemetry;

public record TelemetryStoredEvent(
    Guid DeviceId,
    Guid MessageId,
    DateTime ReceivedAt
);