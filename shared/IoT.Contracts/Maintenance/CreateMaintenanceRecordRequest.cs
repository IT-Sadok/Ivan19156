namespace IoT.Contracts.Maintenance;

public record CreateMaintenanceRecordRequest(
    Guid TechnicianId,
    string? Notes,
    DateTime PerformedAt);