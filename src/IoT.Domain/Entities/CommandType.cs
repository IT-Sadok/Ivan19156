using IoT.Domain.Abstractions;

namespace IoT.Domain.Entities;

public class CommandType : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
}