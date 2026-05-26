using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class MaintenanceRecord : BaseEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid TechnicianId { get; set; }
    public User Technician { get; set; } = null!;

    public string? Notes { get; set; }
    public DateTime PerformedAt { get; set; }
}