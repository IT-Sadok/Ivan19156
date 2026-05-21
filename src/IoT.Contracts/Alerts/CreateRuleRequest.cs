using IoT.Domain.Enums;

namespace IoT.Contracts.Alerts;

public record CreateRuleRequest(
    string Name,
    Guid? DeviceId,
    DeviceType? DeviceType,
    string Field,
    RuleOperator Operator,
    double Value,
    RuleAction Action
);