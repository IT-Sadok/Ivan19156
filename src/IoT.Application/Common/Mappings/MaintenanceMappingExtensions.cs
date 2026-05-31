using IoT.Application.Commands.Maintenance.CreateMaintenanceRecord;
using IoT.Contracts.Maintenance;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public static class MaintenanceMappingExtensions
{
    public static MaintenanceRecordResponse ToResponse(this MaintenanceRecord record) =>
        new(record.Id,
            record.DeviceId,
            record.TechnicianId,
            record.Notes,
            record.PerformedAt,
            record.CreatedAt);

    public static CreateMaintenanceRecordCommand ToCommand(
        this CreateMaintenanceRecordRequest request,
        Guid deviceId) =>
        new(deviceId,
            request.TechnicianId,
            request.Notes,
            request.PerformedAt);
}