using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Enums;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;

namespace IoT.IntegrationTests.Tests.DeviceCommands;

[Collection("Integration")]
public class DeviceCommandTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _adminClient = null!;
    private HttpClient _technicianClient = null!;
    private IoTWebApplicationFactory _factory = null!;

    public DeviceCommandTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _factory = new IoTWebApplicationFactory(_fixture);

        _adminClient = _factory.CreateClient();
        _adminClient.SetBearerToken(TestConstants.TechnicianId, "Admin");

        _technicianClient = _factory.CreateClient();
        _technicianClient.SetBearerToken(TestConstants.TechnicianId, "Technician");

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
        var clientWithoutAuth = _factory.CreateClient();
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
            .PostAsJsonAsync($"/api/devices/{TestConstants.DeviceId}/commands", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<DeviceCommandResponse>();
        body.Should().NotBeNull();
        body!.DeviceId.Should().Be(TestConstants.DeviceId);
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