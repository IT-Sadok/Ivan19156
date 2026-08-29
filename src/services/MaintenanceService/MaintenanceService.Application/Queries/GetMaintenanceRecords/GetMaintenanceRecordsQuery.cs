using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace MaintenanceService.Application.Queries.GetMaintenanceRecords;

public record GetMaintenanceRecordsQuery(Guid DeviceId) : IRequest<Result<IEnumerable<MaintenanceRecordResponse>>>;