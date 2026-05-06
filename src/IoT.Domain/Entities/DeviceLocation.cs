using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class DeviceLocation : BaseEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}