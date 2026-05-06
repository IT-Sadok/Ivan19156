using IoT.Application.Identity.Commands.Register;
using Scalar.AspNetCore;
using IoT.Rest.Extensions;
using IoT.Infrastructure.Extensions;
using IoT.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddMediator(typeof(RegisterCommandHandler).Assembly);
builder.Services.AddSwagger();
builder.Services.AddAuthorization();

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

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();