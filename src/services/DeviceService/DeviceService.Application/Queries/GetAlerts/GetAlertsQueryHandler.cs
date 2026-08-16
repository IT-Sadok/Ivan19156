using DeviceService.Application.Common.Mappings;
using DeviceService.Interfaces;
using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetAlerts;

public class GetAlertsQueryHandler : IRequestHandler<GetAlertsQuery, Result<IEnumerable<AlertResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAlertsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<AlertResponse>>> ExecuteAsync(GetAlertsQuery request, CancellationToken ct = default)
    {
        IEnumerable<DeviceService.Domain.Entities.Alert> alerts;

        if (request.DeviceId.HasValue)
            alerts = await _unitOfWork.Alerts.GetByDeviceAsync(request.DeviceId.Value, ct);
        else
            alerts = await _unitOfWork.Alerts.GetAllAsync(ct);

        var mapped = alerts.Select(a => a.ToDto());
        return Result<IEnumerable<AlertResponse>>.Success(mapped);
    }
}
