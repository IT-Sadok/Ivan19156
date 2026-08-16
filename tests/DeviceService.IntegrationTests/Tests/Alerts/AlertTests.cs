using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IoT.Contracts.Alerts;
using DeviceService.IntegrationTests.Fixtures;
using DeviceService.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceService.IntegrationTests.Tests.Alerts;

[Collection("Integration")]
public class AlertTests : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    private HttpClient _client = null!;

    public AlertTests(IntegrationTestFixture fixture) => _fixture = fixture;

    public Task InitializeAsync()
    {
        _client = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.SetBearerToken(TestConstants.UserId);
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
        var clientWithoutAuth = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
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