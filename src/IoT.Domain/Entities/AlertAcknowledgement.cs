using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class AlertAcknowledgement : BaseEntity
{
    public Guid AlertId { get; set; }
    public Alert Alert { get; set; } = null!;
    public Guid AcknowledgedById { get; set; }
    public User AcknowledgedBy { get; set; } = null!;
    public string? Note { get; set; }
}