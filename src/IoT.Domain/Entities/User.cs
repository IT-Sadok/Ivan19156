using Microsoft.AspNetCore.Identity;

namespace IoT.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
}