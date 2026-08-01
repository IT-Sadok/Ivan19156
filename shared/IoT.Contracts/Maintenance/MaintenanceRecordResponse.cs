namespace IoT.Contracts.Maintenance;

public record MaintenanceRecordResponse(
    Guid Id,
    Guid DeviceId,
    Guid TechnicianId,
    string? Notes,
    DateTime PerformedAt,
    DateTime CreatedAt);