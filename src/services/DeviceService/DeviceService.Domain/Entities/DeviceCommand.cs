using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Entities;

public class DeviceCommand : BaseEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid CommandTypeId { get; set; }
    public CommandType CommandType { get; set; } = null!;

    public Guid? IssuedById { get; set; }

    public string? Payload { get; set; }
    public CommandStatus Status { get; set; } = CommandStatus.Created;

    public DateTime? SentAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
}
