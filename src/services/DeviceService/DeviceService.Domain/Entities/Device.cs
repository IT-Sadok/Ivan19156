using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Events;

namespace DeviceService.Domain.Entities;

public class Device : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public DeviceAdminStatus AdminStatus { get; set; } = DeviceAdminStatus.Active;
    public DateTime? LastSeen { get; set; }

    public Guid? ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }

    public ICollection<DeviceLocation> Locations { get; set; } = [];
    public ICollection<DeviceApiKey> ApiKeys { get; set; } = [];
    public ICollection<DeviceCommand> Commands { get; set; } = [];
    public ICollection<Alert> Alerts { get; set; } = [];

    public static Device Create(string name, DeviceType type, Guid? manufacturerId = null)
    {
        var device = new Device
        {
            Name = name,
            Type = type,
            ManufacturerId = manufacturerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        device.RaiseDomainEvent(new DeviceRegisteredEvent(device.Id, device.Name, device.Type, device.CreatedAt));

        return device;
    }

    public void Delete()
    {
        RaiseDomainEvent(new DeviceDeletedEvent(Id, DateTime.UtcNow));
    }
}
