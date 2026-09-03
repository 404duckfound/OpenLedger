using Microsoft.EntityFrameworkCore;
using OpenLedger.API.Middlewares;
using OpenLedger.API.Services;
using OpenLedger.Application;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;
using OpenLedger.Infrastructure;
using OpenLedger.Infrastructure.Contexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("Base") ?? string.Empty;
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("OpenLedger.Infrastructure"));
});
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context,cancellationToken) =>
    {
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddOptionsWithValidateOnStart<TokenOptions>().BindConfiguration("Token");

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

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
