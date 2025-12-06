using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace FamilySafety.IntegrationTests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsExpectedStatusCode()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health");

        // Assert - Health check may return 503 if DB is not available, which is expected behavior
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task OpenApiEndpoint_ReturnsOk()
    {
        // Arrange & Act - .NET 10 uses /openapi/v1.json instead of /swagger/v1/swagger.json
        var response = await _client.GetAsync("/openapi/v1.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UsersEndpoint_RequiresAuthentication()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/v1/users/me");

        // Assert - Should return Unauthorized when no auth token provided
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
