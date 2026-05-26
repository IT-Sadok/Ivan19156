namespace IoT.Contracts.Identity;

public record RegisterRequest(string UserName, string Email, string Password);
