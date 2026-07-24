using IoT.Application.Commands.Alerts.CreateRule;
using IoT.Contracts.Alerts;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class RuleMappingExtensions
{
    public static Rule ToEntity(this CreateRuleCommand command)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            DeviceId = command.DeviceId,
            DeviceType = command.DeviceType,
            Field = command.Field,
            Operator = command.Operator,
            Value = command.Value,
            Action = command.Action,
            IsActive = true,
            CreatedById = command.CreatedById
        };

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
