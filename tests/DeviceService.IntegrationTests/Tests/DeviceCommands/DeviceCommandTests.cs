using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Enums;
using DeviceService.IntegrationTests.Fixtures;
using DeviceService.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceService.IntegrationTests.Tests.DeviceCommands;

[Collection("Integration")]
public class DeviceCommandTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _adminClient = null!;
    private HttpClient _technicianClient = null!;

    public DeviceCommandTests(IntegrationTestFixture fixture) => _fixture = fixture;

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

        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetCommands_ShouldReturn200()
    {
        var response = await _technicianClient
            .GetAsync($"/api/devices/{TestConstants.DeviceId}/commands");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<DeviceCommandResponse>>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCommands_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var response = await clientWithoutAuth
            .GetAsync($"/api/devices/{TestConstants.DeviceId}/commands");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateCommand_AsAdmin_ShouldReturn200()
    {
        var request = new CreateDeviceCommandRequest(
            CommandTypeSlug: CommandTypeSlug.Reboot,
            Parameters: null,
            Priority: 1,
            ExpiresAt: DateTime.UtcNow.AddHours(1));

        var response = await _adminClient
            .PostAsJsonAsync($"/api/devices/{TestConstants.SeededDeviceId}/commands", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateCommand_AsTechnician_ShouldReturn403()
    {
        var request = new CreateDeviceCommandRequest(
            CommandTypeSlug: CommandTypeSlug.Reboot,
            Parameters: null);

        var response = await _technicianClient
            .PostAsJsonAsync($"/api/devices/{TestConstants.DeviceId}/commands", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}