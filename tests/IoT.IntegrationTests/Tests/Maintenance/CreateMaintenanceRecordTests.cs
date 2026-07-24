using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Maintenance;
using IoT.Infrastructure;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.IntegrationTests.Tests.Maintenance;

[Collection("Integration")]
public class CreateMaintenanceRecordTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private AppDbContext _db = null!;
    private readonly Guid _deviceId = TestConstants.DeviceId;
    private readonly Guid _technicianId = TestConstants.TechnicianId;

    public CreateMaintenanceRecordTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _client = _fixture.Factory.CreateClient();
        _db = _fixture.Factory.Services.CreateScope()
            .ServiceProvider
            .GetRequiredService<AppDbContext>();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateMaintenanceRecord_WithoutNotes_ShouldReturn200()
    {
        var request = new
        {
            technicianId = _technicianId,
            notes = (string?)null,
            performedAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync(
            $"/api/devices/{_deviceId}/maintenance", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<MaintenanceRecordResponse>();
        body.Should().NotBeNull();
        body!.DeviceId.Should().Be(_deviceId);
        body.Notes.Should().BeNull();
    }

    [Fact]
    public async Task CreateMaintenanceRecord_WithNotes_ShouldReturn200AndPersistRecord()
    {
        var request = new
        {
            technicianId = _technicianId,
            notes = "Replaced faulty sensor",
            performedAt = DateTime.UtcNow
        };
        
        var response = await _client.PostAsJsonAsync(
            $"/api/devices/{_deviceId}/maintenance", request);
        var body = await response.Content.ReadFromJsonAsync<MaintenanceRecordResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.Notes.Should().Be(request.notes);
        body.DeviceId.Should().Be(_deviceId);
        
        var record = await _db.MaintenanceRecords.FindAsync(body.Id);
        record.Should().NotBeNull();
        record!.Notes.Should().Be(request.notes);
    }

    [Fact]
    public async Task CreateMaintenanceRecord_WithNotes_ShouldGenerateEmbeddingAsynchronously()
    {

        var request = new
        {
            technicianId = _technicianId,
            notes = "Replaced pressure valve, recalibrated sensors",
            performedAt = DateTime.UtcNow
        };
        
        var response = await _client.PostAsJsonAsync(
            $"/api/devices/{_deviceId}/maintenance", request);
        var body = await response.Content.ReadFromJsonAsync<MaintenanceRecordResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);


        await PollingHelper.WaitUntilAsync(async () =>
        {
            _db.ChangeTracker.Clear();
            var record = await _db.MaintenanceRecords.FindAsync(body!.Id);
            return record?.NotesEmbedding != null;
        }, timeout: TimeSpan.FromSeconds(60));

        _db.ChangeTracker.Clear();
        var finalRecord = await _db.MaintenanceRecords.FindAsync(body!.Id);
        finalRecord.Should().NotBeNull();
        finalRecord!.NotesEmbedding.Should().NotBeNull();
    }
    public Task DisposeAsync() => Task.CompletedTask;
}