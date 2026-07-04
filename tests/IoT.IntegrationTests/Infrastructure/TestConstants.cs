namespace IoT.IntegrationTests.Infrastructure;

public static class TestConstants
{
    public static readonly Guid TechnicianId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000000");
    public static readonly Guid DeviceId = Guid.Parse("b1c2d3e4-0001-0000-0000-000000000000");
    public static class Jwt
    {
        public const string Secret = "test-secret-key-that-is-long-enough-32chars";
        public const string Issuer = "IoT.Api";
        public const string Audience = "IoT.Client";
    }
}