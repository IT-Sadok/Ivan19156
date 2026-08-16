using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Entities;

public class AlertAcknowledgement : BaseEntity
{
    public Guid AlertId { get; set; }
    public Alert Alert { get; set; } = null!;

    public Guid AcknowledgedById { get; set; }
    public DateTime AcknowledgedAt { get; set; }
    public string? Note { get; set; }
}
