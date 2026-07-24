using IoT.Contracts.Maintenance;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Maintenance.GetMaintenanceRecords;

public record GetMaintenanceRecordsQuery(Guid DeviceId) : IRequest<Result<IEnumerable<MaintenanceRecordResponse>>>;