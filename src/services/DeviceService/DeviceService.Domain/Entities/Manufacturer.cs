using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Entities;

public class Manufacturer : AggregateRoot
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Device> Devices { get; set; } = [];
}
