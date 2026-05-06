using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class Manufacturer : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? ContactEmail { get; set; }

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}