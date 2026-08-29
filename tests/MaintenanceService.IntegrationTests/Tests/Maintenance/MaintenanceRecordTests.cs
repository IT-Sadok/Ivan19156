using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Maintenance;
using MaintenanceService.Infrastructure.Persistence;
using MaintenanceService.IntegrationTests.Fixtures;
using MaintenanceService.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MaintenanceService.IntegrationTests.Tests.Maintenance;

[Collection("Integration")]
public class MaintenanceRecordTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _adminClient = null!;
    private HttpClient _technicianClient = null!;
    private MaintenanceDbContext _db = null!;

    public MaintenanceRecordTests(IntegrationTestFixture fixture) => _fixture = fixture;

    public Task InitializeAsync()
    {
        _adminClient = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _adminClient.SetBearerToken(TestConstants.UserId, "Admin");

        _technicianClient = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _technicianClient.SetBearerToken(TestConstants.UserId, "Technician");

        _db = _fixture.Factory.Services.CreateScope()
            .ServiceProvider
            .GetRequiredService<MaintenanceDbContext>();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetMaintenanceRecords_ShouldReturn200()
    {
        var response = await _adminClient
            .GetAsync($"/api/devices/{TestConstants.DeviceId}/maintenance");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<MaintenanceRecordResponse>>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMaintenanceRecords_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var response = await clientWithoutAuth
            .GetAsync($"/api/devices/{TestConstants.DeviceId}/maintenance");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateMaintenanceRecord_ShouldReturn200AndPersist()
    {
        var request = new CreateMaintenanceRecordRequest(
            TechnicianId: TestConstants.TechnicianId,
            Notes: "Replaced faulty sensor",
            PerformedAt: DateTime.UtcNow);

        var response = await _adminClient
            .PostAsJsonAsync($"/api/devices/{TestConstants.DeviceId}/maintenance", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<MaintenanceRecordResponse>();
        body.Should().NotBeNull();
        body!.DeviceId.Should().Be(TestConstants.DeviceId);
        body.Notes.Should().Be(request.Notes);

        _db.ChangeTracker.Clear();
        var record = await _db.MaintenanceRecords.FindAsync(body.Id);
        record.Should().NotBeNull();
        record!.Notes.Should().Be(request.Notes);
    }

    [Fact]
    public async Task CreateMaintenanceRecord_WithoutNotes_ShouldReturn200()
    {
        var request = new CreateMaintenanceRecordRequest(
            TechnicianId: TestConstants.TechnicianId,
            Notes: null,
            PerformedAt: DateTime.UtcNow);

        var response = await _adminClient
            .PostAsJsonAsync($"/api/devices/{TestConstants.DeviceId}/maintenance", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<MaintenanceRecordResponse>();
        body.Should().NotBeNull();
        body!.Notes.Should().BeNull();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}