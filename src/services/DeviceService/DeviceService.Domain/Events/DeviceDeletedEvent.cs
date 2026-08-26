using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Events;

public record DeviceDeletedEvent(
    Guid DeviceId,
    DateTime DeletedAt
) : IDomainEvent;
