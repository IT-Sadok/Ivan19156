using IoT.Application.Common.Mappings;
using IoT.Contracts.Maintenance;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Maintenance.GetMaintenanceRecords;

public class GetMaintenanceRecordsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMaintenanceRecordsQuery, Result<IEnumerable<MaintenanceRecordResponse>>>
{
    public async Task<Result<IEnumerable<MaintenanceRecordResponse>>> Handle(
        GetMaintenanceRecordsQuery request,
        CancellationToken ct)
    {
        var records = await unitOfWork.MaintenanceRecords.GetByDeviceIdAsync(request.DeviceId, ct);
        return Result<IEnumerable<MaintenanceRecordResponse>>.Success(
            records.Select(r => r.ToResponse()));
    }
}