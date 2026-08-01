namespace IoT.Contracts.Identity;

public record AuthResponse(string AccessToken, DateTime ExpiresAt);
