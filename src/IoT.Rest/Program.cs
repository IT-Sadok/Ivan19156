using IoT.Application.Commands.Identity.Register;
using IoT.Infrastructure;
using IoT.Infrastructure.Extensions;
using IoT.Interfaces.Services;
using IoT.Rest.Extensions;
using IoT.Rest.Hubs;
using IoT.Rest.Infrastructure;
using IoT.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiServices();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddMediator(typeof(RegisterCommandHandler).Assembly);
builder.Services.AddSwagger();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddScoped<ICommandHubNotifier, CommandHubNotifier>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddFilters();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await RoleSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithTitle("IoT API")
            .WithTheme(ScalarTheme.Moon));
}
app.MapHub<CommandHub>("/hubs/commands");
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
