// IoT.Shared/Extensions/MiddlewareExtensions.cs
using IoT.Shared.Middleware;
using Microsoft.AspNetCore.Builder;

namespace IoT.Shared.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}