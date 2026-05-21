using IoT.Domain.Abstractions;
using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class Alert : AggregateRoot
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;
    public Guid RuleId { get; set; }
    public Rule Rule { get; set; } = null!;
    public AlertStatus Status { get; set; } = AlertStatus.New;
    public DateTime TriggeredAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public ICollection<AlertAcknowledgement> Acknowledgements { get; set; } = [];
}