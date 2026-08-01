using IoT.Contracts.Alerts;
using IoT.Domain.Enums;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Alerts.CreateRule;

public record CreateRuleCommand(
    string Name,
    Guid? DeviceId,
    DeviceType? DeviceType,
    string Field,
    RuleOperator Operator,
    double Value,
    RuleAction Action,
    Guid CreatedById) : IRequest<Result<RuleResponse>>;