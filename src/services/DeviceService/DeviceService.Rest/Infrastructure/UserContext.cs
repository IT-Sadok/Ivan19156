using System.Security.Claims;
using DeviceService.Interfaces.Services;

namespace DeviceService.Rest.Infrastructure;

public class UserContext : IUserContext
{
    public Guid UserId { get; }

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        var userIdStr = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = Guid.TryParse(userIdStr, out var userId) ? userId : Guid.Empty;
    }
}