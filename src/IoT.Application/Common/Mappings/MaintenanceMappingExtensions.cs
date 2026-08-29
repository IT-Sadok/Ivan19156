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
    
}