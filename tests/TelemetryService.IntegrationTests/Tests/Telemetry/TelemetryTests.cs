using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.IntegrationTests.Fixtures;
using TelemetryService.IntegrationTests.Infrastructure;

namespace TelemetryService.IntegrationTests.Tests.Telemetry;

[Collection("Integration")]
public class TelemetryTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private TelemetryDbContext _db = null!;

    public TelemetryTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _client = _fixture.Factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Api-Key", TestConstants.ApiKey);
        _db = _fixture.Factory.Services.CreateScope()
            .ServiceProvider
            .GetRequiredService<TelemetryDbContext>();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ProcessTelemetry_WithoutApiKey_ShouldReturn401()
    {
        var clientWithoutKey = _fixture.Factory.CreateClient();
        var request = new
        {
            messageId = Guid.NewGuid(),
            payload = """{"temperature": 20.0}"""
        };

        var response = await clientWithoutKey.PostAsJsonAsync("/api/telemetry", request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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

        TelemetryService.Domain.Entities.TelemetryRecord? record = null;
        await PollingHelper.WaitUntilAsync(async () =>
        {
            _db.ChangeTracker.Clear();
            record = await _db.TelemetryRecords
                .FirstOrDefaultAsync(t =>
                    t.DeviceId == TelemetryService.Domain.ValueObjects.DeviceId.From(TestConstants.DeviceId)
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
            var count = await _db.TelemetryRecords
                .CountAsync(t =>
                    t.DeviceId == TelemetryService.Domain.ValueObjects.DeviceId.From(TestConstants.DeviceId)
                    && t.MessageId == messageId);
            return count > 0;
        }, timeout: TimeSpan.FromSeconds(15));

        _db.ChangeTracker.Clear();
        var finalCount = await _db.TelemetryRecords
            .CountAsync(t =>
                t.DeviceId == TelemetryService.Domain.ValueObjects.DeviceId.From(TestConstants.DeviceId)
                && t.MessageId == messageId);
        finalCount.Should().Be(1);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}