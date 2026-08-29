using Microsoft.EntityFrameworkCore;
using OpenLedger.API.Services;
using OpenLedger.Application;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;
using OpenLedger.Infrastructure;
using OpenLedger.Infrastructure.Contexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddOptionsWithValidateOnStart<TokenOptions>().BindConfiguration("Token");

builder.Services.AddDbContext<AppDbContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("2026") ?? string.Empty;
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("OpenLedger.Infrastructure"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
