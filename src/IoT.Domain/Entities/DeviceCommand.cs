using IoT.Domain.Abstractions;
using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class DeviceCommand : AggregateRoot
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid? IssuedById { get; set; }
    public User? IssuedBy { get; set; }

    public Guid CommandTypeId { get; set; }
    public CommandType CommandType { get; set; } = null!;

    public string? Parameters { get; set; }
    public int Priority { get; set; } = 0;
    public CommandStatus Status { get; set; } = CommandStatus.Created;

    public DateTime? SentAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}