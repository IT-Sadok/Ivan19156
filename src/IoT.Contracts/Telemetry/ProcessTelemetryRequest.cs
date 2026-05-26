namespace IoT.Contracts.Telemetry;

public record ProcessTelemetryRequest(
    Guid DeviceId,
    Guid MessageId,
    string Payload
);
