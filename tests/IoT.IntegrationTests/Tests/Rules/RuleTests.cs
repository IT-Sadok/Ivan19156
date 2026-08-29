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

    public RuleTests(IntegrationTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync()
    {
        _client = _fixture.Factory.CreateClient();
        _client.SetBearerToken(TestConstants.TechnicianId, "Technician");

        _adminClient = _fixture.Factory.CreateClient();
        _adminClient.SetBearerToken(TestConstants.TechnicianId, "Admin");

        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetRules_WithoutAuth_ShouldReturn401()
    {
        var clientWithoutAuth = _fixture.Factory.CreateClient(); 
        var response = await clientWithoutAuth.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task GetRules_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/rules");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<IEnumerable<RuleResponse>>();
        body.Should().NotBeNull();
    }

    

    
    public Task DisposeAsync() => Task.CompletedTask;
}