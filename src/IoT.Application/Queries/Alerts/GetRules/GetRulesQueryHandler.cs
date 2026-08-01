// IoT.Application/Queries/Alerts/GetRules/GetRulesQueryHandler.cs
using IoT.Contracts.Alerts;
using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Queries.Alerts.GetRules;

public class GetRulesQueryHandler
    : IRequestHandler<GetRulesQuery, Result<IEnumerable<RuleResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRulesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<RuleResponse>>> Handle(
        GetRulesQuery request,
        CancellationToken ct = default)
    {
        var rules = await _unitOfWork.Rules
            .Filter(noTracking: true)
            .ToListAsync(ct);

        var response = rules.Select(r => new RuleResponse(
            r.Id,
            r.Name,
            r.DeviceId,
            r.DeviceType,
            r.Field,
            r.Operator,
            r.Value,
            r.Action,
            r.IsActive,
            r.CreatedAt
        ));

        return Result<IEnumerable<RuleResponse>>.Success(response);
    }
}