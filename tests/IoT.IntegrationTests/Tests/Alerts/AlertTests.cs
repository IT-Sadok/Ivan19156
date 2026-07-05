using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Alerts;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;

namespace IoT.IntegrationTests.Tests.Alerts;

[Collection("Integration")]
public class AlertTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private IoTWebApplicationFactory _factory = null!;

    public AlertTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _factory = new IoTWebApplicationFactory(_fixture);
        _client = _factory.CreateClient();
        _client.SetBearerToken(TestConstants.TechnicianId);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetAlerts_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/alerts");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<IEnumerable<AlertResponse>>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAlerts_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _factory.CreateClient();
        var response = await clientWithoutAuth.GetAsync("/api/alerts");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAlerts_FilterByDeviceId_ShouldReturn200()
    {
        var response = await _client.GetAsync($"/api/alerts?deviceId={TestConstants.DeviceId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<IEnumerable<AlertResponse>>();
        body.Should().NotBeNull();
        body!.Should().OnlyContain(a => a.DeviceId == TestConstants.DeviceId);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}