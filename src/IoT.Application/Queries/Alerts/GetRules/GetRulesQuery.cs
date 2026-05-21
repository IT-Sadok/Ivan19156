using IoT.Contracts.Alerts;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Alerts.GetRules;

public record GetRulesQuery : IRequest<Result<IEnumerable<RuleResponse>>>;