using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Alerts;
using IoT.Domain.Enums;
using DeviceService.IntegrationTests.Fixtures;
using DeviceService.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceService.IntegrationTests.Tests.Rules;

[Collection("Integration")]
public class RuleTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _userClient = null!;
    private HttpClient _adminClient = null!;

    public RuleTests(IntegrationTestFixture fixture) => _fixture = fixture;

    public Task InitializeAsync()
    {
        _userClient = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _userClient.SetBearerToken(TestConstants.UserId, "Technician");

        _adminClient = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _adminClient.SetBearerToken(TestConstants.UserId, "Admin");

        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetRules_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var response = await clientWithoutAuth.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetRules_ShouldReturn200()
    {
        var response = await _userClient.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<RuleResponse>>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateRule_AsAdmin_ShouldReturn200()
    {
        var request = new
        {
            name = "High Temperature Alert",
            deviceId = TestConstants.SeededDeviceId,
            deviceType = (DeviceType?)null,
            field = "temperature",
            @operator = RuleOperator.Gt,
            value = 50.0,
            action = RuleAction.CreateAlert
        };

        var response = await _adminClient.PostAsJsonAsync("/api/rules", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateRule_AsTechnician_ShouldReturn403()
    {
        var request = new
        {
            name = "Test Rule",
            deviceId = TestConstants.DeviceId,
            deviceType = (DeviceType?)null,
            field = "temperature",
            @operator = RuleOperator.Gt,
            value = 50.0,
            action = RuleAction.CreateAlert
        };

        var response = await _userClient.PostAsJsonAsync("/api/rules", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}