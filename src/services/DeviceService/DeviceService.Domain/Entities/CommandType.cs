using DeviceService.Domain.Abstractions;

namespace DeviceService.Domain.Entities;

public class CommandType : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
}
