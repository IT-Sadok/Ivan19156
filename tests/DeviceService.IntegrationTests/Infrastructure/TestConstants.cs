namespace DeviceService.IntegrationTests.Infrastructure;

public static class TestConstants
{
    public static readonly Guid DeviceId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public const string Secret = "test-signing-key-at-least-32-chars!!";
    public static readonly Guid UserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public const string ApiKey = "iot_testkey_test-api-key-for-integration-tests";
    public static readonly Guid SeededDeviceId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    
    public static class Jwt
    {
        public const string Secret = "test-signing-key-at-least-32-chars!!";
        public const string Issuer = "test-issuer";
        public const string Audience = "test-audience";
    }

}
