using IoT.Domain.Enums;

namespace IoT.Contracts.Alerts;

public record AlertResponse(
    Guid Id,
    Guid DeviceId,
    Guid RuleId,
    string RuleName,
    AlertStatus Status,
    DateTime TriggeredAt,
    DateTime? ResolvedAt
);