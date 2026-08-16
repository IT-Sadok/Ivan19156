using DeviceService.Application.Common.Mappings;
using DeviceService.Interfaces;
using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetRules;

public class GetRulesQueryHandler : IRequestHandler<GetRulesQuery, Result<IEnumerable<RuleResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRulesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<RuleResponse>>> ExecuteAsync(GetRulesQuery request, CancellationToken ct = default)
    {
        var rules = await _unitOfWork.Rules.GetAllAsync(ct);

        if (request.DeviceId.HasValue)
            rules = rules.Where(r => r.DeviceId == request.DeviceId.Value);

        var mapped = rules.Select(r => r.ToDto());
        return Result<IEnumerable<RuleResponse>>.Success(mapped);
    }
}
