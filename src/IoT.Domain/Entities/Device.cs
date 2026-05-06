using IoT.Domain.Abstractions;
using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class Device : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public DeviceAdminStatus AdminStatus { get; set; } = DeviceAdminStatus.Active;
    public DateTime? LastSeen { get; set; }

    public Guid? ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }

    public ICollection<DeviceLocation> Locations { get; set; } = new List<DeviceLocation>();
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
}