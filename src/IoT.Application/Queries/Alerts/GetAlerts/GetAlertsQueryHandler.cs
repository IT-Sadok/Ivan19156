using IoT.Contracts.Alerts;
using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Queries.Alerts.GetAlerts;

public class GetAlertsQueryHandler
    : IRequestHandler<GetAlertsQuery, Result<IEnumerable<AlertResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAlertsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<AlertResponse>>> ExecuteAsync(
        GetAlertsQuery request,
        CancellationToken ct = default)
    {
        var query = _unitOfWork.Alerts
            .Filter(noTracking: true);

        if (request.DeviceId.HasValue)
            query = query.Where(a => a.DeviceId == request.DeviceId.Value);

        var alerts = await query
            .Include(a => a.Rule)
            .ToListAsync(ct);

        var response = alerts.Select(a => new AlertResponse(
            a.Id,
            a.DeviceId,
            a.RuleId,
            a.Rule.Name,
            a.Status,
            a.TriggeredAt,
            a.ResolvedAt
        ));

        return Result<IEnumerable<AlertResponse>>.Success(response);
    }
}