using MaintenanceService.Domain.Abstractions;
using MaintenanceService.Domain.ValueObjects;

namespace MaintenanceService.Domain.Entities;

public class MaintenanceRecord : AggregateRoot
{
    public DeviceId DeviceId { get; set; } = null!;
    public TechnicianId TechnicianId { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime PerformedAt { get; set; }
    public float[]? NotesEmbedding { get; set; }

    public static MaintenanceRecord Create(
        DeviceId deviceId,
        TechnicianId technicianId,
        string? notes,
        DateTime performedAt)
    {
        return new MaintenanceRecord
        {
            DeviceId = deviceId,
            TechnicianId = technicianId,
            Notes = notes,
            PerformedAt = performedAt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}