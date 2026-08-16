using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Entities;

public class Alert : BaseEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public Guid RuleId { get; set; }
    public Rule Rule { get; set; } = null!;

    public string Message { get; set; } = string.Empty;
    public AlertStatus Status { get; set; } = AlertStatus.New;
    public double TriggeredValue { get; set; }

    public ICollection<AlertAcknowledgement> Acknowledgements { get; set; } = [];
}
