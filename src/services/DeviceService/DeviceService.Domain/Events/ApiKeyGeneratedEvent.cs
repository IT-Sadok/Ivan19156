using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Events;

public record ApiKeyGeneratedEvent(
    Guid DeviceId,
    Guid ApiKeyId,
    DateTime GeneratedAt
) : IDomainEvent;
