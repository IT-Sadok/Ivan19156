using IoT.Shared.Infrastructure;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TelemetryService.Interfaces.Services;

namespace TelemetryService.Rest.Infrastructure;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyAuthFilter(IApiKeyService apiKeyService) => _apiKeyService = apiKeyService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        
        if (context.HttpContext.Request.Headers.TryGetValue("X-Device-Id", out var deviceIdHeader)
            && Guid.TryParse(deviceIdHeader, out var gatewayDeviceId))
        {
            context.HttpContext.Items["DeviceId"] = gatewayDeviceId;
            await next();
            return;
        }

        
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiConstants.ApiKeyHeader, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var deviceId = await _apiKeyService.ValidateAsync(apiKey!);
        if (deviceId is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["DeviceId"] = deviceId;
        await next();
    }
} 
