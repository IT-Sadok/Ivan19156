using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Entities;

public class Warehouse : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }

    public ICollection<DeviceLocation> DeviceLocations { get; set; } = [];
}
