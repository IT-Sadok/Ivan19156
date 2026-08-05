using TelemetryService.Application.Commands.ProcessTelemetry;
using TelemetryService.Rest.Infrastructure;
using TelemetryService.Infrastructure.Extensions;
using IoT.Shared.Mediator;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.Rest.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddMediator();
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();
    await db.Database.MigrateAsync();
}
 
app.Run();