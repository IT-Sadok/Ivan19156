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

    public DeviceCommandTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _adminClient = _fixture.Factory.CreateClient();
        _adminClient.SetBearerToken(TestConstants.TechnicianId, "Admin");

        _technicianClient = _fixture.Factory.CreateClient();
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
        var clientWithoutAuth = _fixture.Factory.CreateClient();
        var response = await clientWithoutAuth
            .GetAsync($"/api/devices/{TestConstants.DeviceId}/commands");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

   

    
    public Task DisposeAsync() => Task.CompletedTask;
}