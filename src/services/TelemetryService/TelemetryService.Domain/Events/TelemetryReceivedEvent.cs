using TelemetryService.Domain.Abstractions;
using TelemetryService.Domain.ValueObjects;

namespace TelemetryService.Domain.Events;

public record TelemetryReceivedEvent(
    DeviceId DeviceId,
    Guid MessageId,
    string Payload,
    DateTime ReceivedAt
) : IDomainEvent;