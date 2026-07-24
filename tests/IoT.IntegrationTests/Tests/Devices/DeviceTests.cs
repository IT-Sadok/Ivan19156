using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Infrastructure;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;
using IoT.Shared.Common;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.IntegrationTests.Tests.Devices;

[Collection("Integration")]
public class DeviceTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private AppDbContext _db = null!;

    public DeviceTests(IntegrationTestFixture fixture)
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
    public async Task GetDevices_ShouldReturn200WithPagedResult()
    {
        var response = await _client.GetAsync("/api/devices?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceResponse>>();
        body.Should().NotBeNull();
        body!.Items.Should().NotBeEmpty(); 
    }
    
    [Fact]
    public async Task GetDeviceById_ExistingDevice_ShouldReturn200()
    {
        var response = await _client.GetAsync($"/api/devices/{TestConstants.DeviceId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeviceResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(TestConstants.DeviceId);
    }

    [Fact]
    public async Task GetDeviceById_NonExistingDevice_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/devices/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateDevice_ValidRequest_ShouldReturn201AndPersistDevice()
    {
        var request = new CreateDeviceRequest(
            Name: "New Test Sensor",
            Type: DeviceType.Sensor,
            ManufacturerId: null);

        var response = await _client.PostAsJsonAsync("/api/devices", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<DeviceResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be(request.Name);
        
        var device = await _db.Devices.FindAsync(body.Id);
        device.Should().NotBeNull();
        device!.Name.Should().Be(request.Name);
    }
    
    [Fact]
    public async Task DeleteDevice_ExistingDevice_ShouldReturn200AndRemoveFromDb()
    {
        var createRequest = new CreateDeviceRequest(
            Name: "Device To Delete",
            Type: DeviceType.Sensor,
            ManufacturerId: null);

        var createResponse = await _client.PostAsJsonAsync("/api/devices", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<DeviceResponse>();
        
        var deleteResponse = await _client.DeleteAsync($"/api/devices/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        _db.ChangeTracker.Clear();
        var device = await _db.Devices.FindAsync(created.Id);
        device.Should().BeNull();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}