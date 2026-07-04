using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Domain.Entities;
using IoT.Infrastructure;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.IntegrationTests.Tests.Telemetry;

[Collection("Integration")]
public class TelemetryTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private AppDbContext _db = null!;
    private IoTWebApplicationFactory _factory = null!;

    public TelemetryTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _factory = new IoTWebApplicationFactory(_fixture);
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Api-Key", "test-api-key");
        _db = _factory.Services.CreateScope()
            .ServiceProvider
            .GetRequiredService<AppDbContext>();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ProcessTelemetry_ValidPayload_ShouldReturn200AndPersistRecord()
    {
        var messageId = Guid.NewGuid();
        var request = new
        {
            messageId,
            payload = """{"temperature": 22.5}"""
        };

        var response = await _client.PostAsJsonAsync("/api/telemetry", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        TelemetryRecord? record = null;
        await PollingHelper.WaitUntilAsync(async () =>
        {
            _db.ChangeTracker.Clear();
            record = await _db.Set<TelemetryRecord>()
                .FirstOrDefaultAsync(t => t.DeviceId == TestConstants.DeviceId
                                       && t.MessageId == messageId);
            return record != null;
        }, timeout: TimeSpan.FromSeconds(15));

        record.Should().NotBeNull();
        record!.Payload.Should().Be(request.payload);
    }

    [Fact]
    public async Task ProcessTelemetry_DuplicateMessageId_ShouldBeIdempotent()
    {
        var messageId = Guid.NewGuid();
        var request = new
        {
            messageId,
            payload = """{"temperature": 25.0}"""
        };

        await _client.PostAsJsonAsync("/api/telemetry", request);
        await _client.PostAsJsonAsync("/api/telemetry", request);

        await PollingHelper.WaitUntilAsync(async () =>
        {
            _db.ChangeTracker.Clear();
            var count = await _db.Set<TelemetryRecord>()
                .CountAsync(t => t.DeviceId == TestConstants.DeviceId
                              && t.MessageId == messageId);
            return count > 0;
        }, timeout: TimeSpan.FromSeconds(15));

        _db.ChangeTracker.Clear();
        var finalCount = await _db.Set<TelemetryRecord>()
            .CountAsync(t => t.DeviceId == TestConstants.DeviceId
                          && t.MessageId == messageId);
        finalCount.Should().Be(1);
    }

    [Fact]
    public async Task ProcessTelemetry_ValidPayload_ShouldUpdateDeviceLastSeen()
    {
        var request = new
        {
            messageId = Guid.NewGuid(),
            payload = """{"humidity": 60}"""
        };

        await _client.PostAsJsonAsync("/api/telemetry", request);

        Device? device = null;
        await PollingHelper.WaitUntilAsync(async () =>
        {
            _db.ChangeTracker.Clear();
            device = await _db.Set<Device>().FindAsync(TestConstants.DeviceId);
            return device?.LastSeen != null;
        }, timeout: TimeSpan.FromSeconds(15));

        device!.LastSeen.Should().NotBeNull();
    }

    [Fact]
    public async Task ProcessTelemetry_WithoutApiKey_ShouldReturn401()
    {
        var clientWithoutKey = _factory.CreateClient();
        var request = new
        {
            messageId = Guid.NewGuid(),
            payload = """{"temperature": 20.0}"""
        };

        var response = await clientWithoutKey.PostAsJsonAsync("/api/telemetry", request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}