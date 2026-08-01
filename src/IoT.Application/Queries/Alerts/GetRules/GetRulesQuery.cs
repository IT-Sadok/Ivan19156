using IoT.Contracts.Alerts;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Alerts.GetRules;

public record GetRulesQuery : IRequest<Result<IEnumerable<RuleResponse>>>;