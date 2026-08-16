using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Entities;

public class DeviceApiKey : BaseEntity
{
    public Guid DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public string KeyHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public string Prefix { get; set; } = string.Empty;
}
