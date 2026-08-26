using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Shared.Common;
using DeviceService.Infrastructure.Persistence;
using DeviceService.IntegrationTests.Fixtures;
using DeviceService.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceService.IntegrationTests.Tests.Devices;

[Collection("Integration")]
public class DeviceTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _adminClient = null!;
    private HttpClient _technicianClient = null!;
    private DeviceDbContext _db = null!;

    public DeviceTests(IntegrationTestFixture fixture) => _fixture = fixture;

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
            .GetRequiredService<DeviceDbContext>();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetDevices_ShouldReturn200WithPagedResult()
    {
        var response = await _adminClient.GetAsync("/api/devices?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceResponse>>();
        body.Should().NotBeNull();
        body!.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetDeviceById_ExistingDevice_ShouldReturn200()
    {
        var response = await _adminClient.GetAsync($"/api/devices/{TestConstants.SeededDeviceId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeviceResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(TestConstants.SeededDeviceId);
    }

    [Fact]
    public async Task GetDeviceById_NonExistingDevice_ShouldReturn400()
    {
        var response = await _adminClient.GetAsync($"/api/devices/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateDevice_AsAdmin_ShouldReturn200AndPersistDevice()
    {
        var request = new CreateDeviceRequest(
            Name: "New Test Sensor",
            Type: DeviceType.Sensor,
            ManufacturerId: null);

        var response = await _adminClient.PostAsJsonAsync("/api/devices", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();

        _db.ChangeTracker.Clear();
        var device = await _db.Devices.FindAsync(id);
        device.Should().NotBeNull();
        device!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task CreateDevice_AsTechnician_ShouldReturn403()
    {
        var request = new CreateDeviceRequest(
            Name: "Unauthorized Sensor",
            Type: DeviceType.Sensor,
            ManufacturerId: null);

        var response = await _technicianClient.PostAsJsonAsync("/api/devices", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteDevice_AsAdmin_ShouldReturn200AndRemoveFromDb()
    {
        var createRequest = new CreateDeviceRequest(
            Name: "Device To Delete",
            Type: DeviceType.Sensor,
            ManufacturerId: null);

        var createResponse = await _adminClient.PostAsJsonAsync("/api/devices", createRequest);
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var deleteResponse = await _adminClient.DeleteAsync($"/api/devices/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        _db.ChangeTracker.Clear();
        var device = await _db.Devices.FindAsync(id);
        device.Should().BeNull();
    }

    [Fact]
    public async Task GetDevices_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var response = await clientWithoutAuth.GetAsync("/api/devices");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}