namespace IoT.Contracts.Telemetry;

public record ProcessTelemetryRequest(
    Guid MessageId,
    string Payload
);