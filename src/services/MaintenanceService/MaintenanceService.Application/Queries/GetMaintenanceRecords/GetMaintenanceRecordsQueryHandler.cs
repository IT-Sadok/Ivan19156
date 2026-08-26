using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using MaintenanceService.Application.Common.Mappings;
using MaintenanceService.Interfaces;

namespace MaintenanceService.Application.Queries.GetMaintenanceRecords;

public class GetMaintenanceRecordsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMaintenanceRecordsQuery, Result<IEnumerable<MaintenanceRecordResponse>>>
{
    public async Task<Result<IEnumerable<MaintenanceRecordResponse>>> ExecuteAsync(
        GetMaintenanceRecordsQuery request,
        CancellationToken ct)
    {
        var records = await unitOfWork.MaintenanceRecords.GetByDeviceIdAsync(request.DeviceId, ct);
        return Result<IEnumerable<MaintenanceRecordResponse>>.Success(records.Select(r => r.ToResponse()));
    }
}