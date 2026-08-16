namespace IoT.Contracts.Events;

public record ApiKeyGeneratedEvent(
    Guid DeviceId,
    Guid ApiKeyId,
    DateTime GeneratedAt);
