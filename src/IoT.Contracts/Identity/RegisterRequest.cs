using IoT.Domain.Enums;

namespace IoT.Contracts.Identity;

public record RegisterRequest(string UserName, string Email, string Password,  UserRole Role = UserRole.Customer);
