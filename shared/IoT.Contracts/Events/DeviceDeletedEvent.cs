namespace IoT.Contracts.Events;

public record DeviceDeletedEvent(
    Guid DeviceId,
    DateTime DeletedAt);
