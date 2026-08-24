using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenLedger.API.Services;
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

builder.Services.AddDbContext<AppDbContext>((services, options) =>
{
    var dbOptions = services.GetRequiredService<IOptions<DbOptions>>().Value;
    options.UseNpgsql(dbOptions.ConnectionString);
});

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

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
