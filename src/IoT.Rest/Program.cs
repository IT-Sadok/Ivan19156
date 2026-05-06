using IoT.Application.Commands.Identity.Register;
using Scalar.AspNetCore;
using IoT.Rest.Extensions;
using IoT.Infrastructure.Extensions;
using IoT.Interfaces.Services;
using IoT.Rest.Hubs;
using IoT.Rest.Infrastructure;
using IoT.Shared.Extensions;

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

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.MapScalarApiReference();
// }
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}
app.MapHub<CommandHub>("/hubs/commands");
app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();