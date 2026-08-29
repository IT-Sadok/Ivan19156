using IoT.Shared.Infrastructure;
using System.Net.Http.Json;

namespace IoTGateway.Middleware;

public class ApiKeyValidationMiddleware : IMiddleware
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ApiKeyValidationMiddleware> _logger;

    public ApiKeyValidationMiddleware(
        IHttpClientFactory httpClientFactory,
        ILogger<ApiKeyValidationMiddleware> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue(ApiConstants.ApiKeyHeader, out var apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var client = _httpClientFactory.CreateClient("DeviceService");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/internal/api-keys/validate");
        request.Headers.Add(ApiConstants.ApiKeyHeader, apiKey.ToString());

        try
        {
            var response = await client.SendAsync(request, context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var result = await response.Content.ReadFromJsonAsync<ValidateApiKeyResponse>(
                context.RequestAborted);

            if (result is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            context.Items["DeviceId"] = result.DeviceId;
            context.Request.Headers["X-Device-Id"] = result.DeviceId.ToString();
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate API key");
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        }
    }

    private record ValidateApiKeyResponse(Guid DeviceId);
}