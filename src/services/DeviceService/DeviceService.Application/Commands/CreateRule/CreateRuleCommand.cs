using DeviceService.Domain.Enums;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.CreateRule;

public record CreateRuleCommand(
    string Name,
    string MetricKey,
    RuleOperator Operator,
    double Threshold,
    RuleAction Action,
    Guid? DeviceId,
    DeviceType? DeviceType) : IRequest<Result<Guid>>;
