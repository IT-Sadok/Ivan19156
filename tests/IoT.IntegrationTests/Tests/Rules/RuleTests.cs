using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts;
using IoT.Contracts.Alerts;
using IoT.Domain.Enums;
using IoT.IntegrationTests.Fixtures;
using IoT.IntegrationTests.Infrastructure;

namespace IoT.IntegrationTests.Tests.Rules;

[Collection("Integration")]
public class RuleTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;
    private HttpClient _adminClient = null!;
    private IoTWebApplicationFactory _factory = null!;

    public RuleTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _factory = new IoTWebApplicationFactory(_fixture);

        _client = _factory.CreateClient();
        _client.SetBearerToken(TestConstants.TechnicianId, "Technician");

        _adminClient = _factory.CreateClient();
        _adminClient.SetBearerToken(TestConstants.TechnicianId, "Admin");

        return Task.CompletedTask;
    }
    [Fact]
    public async Task GetRules_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<IEnumerable<RuleResponse>>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRules_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _factory.CreateClient();
        var response = await clientWithoutAuth.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateRule_AsAdmin_ShouldReturn200()
    {
        var request = new
        {
            name = "High Temperature Alert",
            deviceId = TestConstants.DeviceId,
            deviceType = (DeviceType?)null,
            field = "temperature",
            @operator = RuleOperator.Gt,
            value = 50.0,
            action = RuleAction.CreateAlert
        };

        var response = await _adminClient.PostAsJsonAsync("/api/rules", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<RuleResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be(request.name);
        body.DeviceId.Should().Be(TestConstants.DeviceId);
    }

    [Fact]
    public async Task CreateRule_AsTechnician_ShouldReturn403()
    {
        var request = new
        {
            name = "Test Rule",
            deviceId = TestConstants.DeviceId,
            deviceType = DeviceType.Sensor,
            field = "temperature",
            @operator = RuleOperator.Gt,
            value = 50.0,
            action = RuleAction.CreateAlert
        };

        var response = await _client.PostAsJsonAsync("/api/rules", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}