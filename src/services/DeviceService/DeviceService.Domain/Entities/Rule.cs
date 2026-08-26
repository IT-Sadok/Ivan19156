using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Entities;

public class Rule : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string MetricKey { get; set; } = string.Empty;
    public RuleOperator Operator { get; set; }
    public double Threshold { get; set; }
    public RuleAction Action { get; set; }

    public Guid? DeviceId { get; set; }
    public DeviceType? DeviceType { get; set; }

    public bool IsActive { get; set; } = true;
}
