using IoT.Contracts.Maintenance;
using MaintenanceService.Domain.Entities;

namespace MaintenanceService.Application.Common.Mappings;

public static class MaintenanceRecordMappings
{
    public static MaintenanceRecordResponse ToResponse(this MaintenanceRecord record) => new(
        record.Id,
        record.DeviceId.Value,
        record.TechnicianId.Value,
        record.Notes,
        record.PerformedAt,
        record.CreatedAt);
}