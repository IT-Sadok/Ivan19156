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

    

    public Task DisposeAsync() => Task.CompletedTask;
}