using DeviceService.Interfaces.Services;
using IoT.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeviceService.Rest.Infrastructure;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyAuthFilter(IApiKeyService apiKeyService) => _apiKeyService = apiKeyService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
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
