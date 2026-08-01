namespace IoT.Contracts.Telemetry;

public record TelemetryRecentSummaryResponse(
    Guid DeviceId,
    int Count,
    DateTimeOffset LastAt);