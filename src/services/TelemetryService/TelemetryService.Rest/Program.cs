using TelemetryService.Application.Commands.ProcessTelemetry;
using TelemetryService.Rest.Infrastructure;
using TelemetryService.Infrastructure.Extensions;
using IoT.Shared.Mediator;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithTitle("Telemetry API")
            .WithTheme(ScalarTheme.Moon));
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();