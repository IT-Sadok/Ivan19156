using IoT.Contracts.Maintenance;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Maintenance.CreateMaintenanceRecord;

public record CreateMaintenanceRecordCommand(
    Guid DeviceId,
    Guid TechnicianId,
    string? Notes,
    DateTime PerformedAt) : IRequest<Result<MaintenanceRecordResponse>>;