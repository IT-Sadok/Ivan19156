using IoT.Domain.Abstractions;
using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class Rule : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public Guid? DeviceId { get; set; }
    public Device? Device { get; set; }
    public DeviceType? DeviceType { get; set; }
    public string Field { get; set; } = string.Empty;
    public RuleOperator Operator { get; set; }
    public double Value { get; set; }
    public RuleAction Action { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
}