namespace MaintenanceService.IntegrationTests.Infrastructure;

public static class TestConstants
{
    public static readonly Guid DeviceId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid TechnicianId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid UserId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static class Jwt
    {
        public const string Secret = "test-signing-key-at-least-32-chars!!";
        public const string Issuer = "test-issuer";
        public const string Audience = "test-audience";
    }
}