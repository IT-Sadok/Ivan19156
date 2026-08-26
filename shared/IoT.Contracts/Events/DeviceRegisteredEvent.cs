namespace IoT.Contracts.Events;

public record DeviceRegisteredEvent(
    Guid DeviceId,
    string Name,
    string Type,
    DateTime RegisteredAt);
