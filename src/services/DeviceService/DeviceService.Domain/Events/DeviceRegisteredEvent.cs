using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Events;

public record DeviceRegisteredEvent(
    Guid DeviceId,
    string Name,
    DeviceType Type,
    DateTime RegisteredAt
) : IDomainEvent;
