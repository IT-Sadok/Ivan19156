using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetRules;

public record GetRulesQuery(Guid? DeviceId = null) : IRequest<Result<IEnumerable<RuleResponse>>>;
