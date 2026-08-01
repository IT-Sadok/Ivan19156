using IoT.Domain.Enums;

namespace IoT.Contracts.Alerts;

public record RuleResponse(
    Guid Id,
    string Name,
    Guid? DeviceId,
    DeviceType? DeviceType,
    string Field,
    RuleOperator Operator,
    double Value,
    RuleAction Action,
    bool IsActive,
    DateTime CreatedAt
);