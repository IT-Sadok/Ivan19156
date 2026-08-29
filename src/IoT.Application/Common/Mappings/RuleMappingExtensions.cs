using IoT.Contracts.Alerts;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class RuleMappingExtensions
{
    

    public static RuleResponse ToResponse(this Rule rule)
        => new(
            rule.Id,
            rule.Name,
            rule.DeviceId,
            rule.DeviceType,
            rule.Field,
            rule.Operator,
            rule.Value,
            rule.Action,
            rule.IsActive,
            rule.CreatedAt);
}
