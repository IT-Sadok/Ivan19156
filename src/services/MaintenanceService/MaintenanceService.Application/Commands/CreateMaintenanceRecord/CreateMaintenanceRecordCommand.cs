using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace MaintenanceService.Application.Commands.CreateMaintenanceRecord;

public record CreateMaintenanceRecordCommand(
    Guid DeviceId,
    Guid TechnicianId,
    string? Notes,
    DateTime PerformedAt) : IRequest<Result<MaintenanceRecordResponse>>;