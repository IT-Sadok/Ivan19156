using DeviceService.Domain.Entities;
using IoT.Contracts.Alerts;
using IoT.Domain.Enums;

namespace DeviceService.Application.Common.Mappings;

public static class RuleMappings
{
    public static RuleResponse ToDto(this Rule rule) => new(
        rule.Id,
        rule.Name,
        rule.DeviceId,
        MapDeviceType(rule.DeviceType),
        rule.MetricKey,
        MapRuleOperator(rule.Operator),
        rule.Threshold,
        MapRuleAction(rule.Action),
        rule.IsActive,
        rule.CreatedAt);

    private static DeviceType? MapDeviceType(DeviceService.Domain.Enums.DeviceType? type) => type switch
    {
        DeviceService.Domain.Enums.DeviceType.Sensor => DeviceType.Sensor,
        DeviceService.Domain.Enums.DeviceType.Actuator => DeviceType.Actuator,
        null => null,
        _ => null
    };

    private static RuleOperator MapRuleOperator(DeviceService.Domain.Enums.RuleOperator op) => op switch
    {
        DeviceService.Domain.Enums.RuleOperator.Gt  => RuleOperator.Gt,
        DeviceService.Domain.Enums.RuleOperator.Lt  => RuleOperator.Lt,
        DeviceService.Domain.Enums.RuleOperator.Eq  => RuleOperator.Eq,
        DeviceService.Domain.Enums.RuleOperator.Neq => RuleOperator.Neq,
        DeviceService.Domain.Enums.RuleOperator.Gte => RuleOperator.Gte,
        DeviceService.Domain.Enums.RuleOperator.Lte => RuleOperator.Lte,
        _ => RuleOperator.Gt
    };

    private static RuleAction MapRuleAction(DeviceService.Domain.Enums.RuleAction action) => action switch
    {
        DeviceService.Domain.Enums.RuleAction.CreateAlert => RuleAction.CreateAlert,
        DeviceService.Domain.Enums.RuleAction.SendEmail => RuleAction.SendEmail,
        _ => RuleAction.CreateAlert
    };
}
