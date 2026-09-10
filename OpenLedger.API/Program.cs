using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenLedger.API.Middlewares;
using OpenLedger.API.Services;
using OpenLedger.Application;
using OpenLedger.Application.Exceptions;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;
using OpenLedger.Domain.Constants;
using OpenLedger.Infrastructure;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddOptionsWithValidateOnStart<TokenOptions>().BindConfiguration("Token");
var tokenOptions = builder.Configuration.GetSection("Token").Get<TokenOptions>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        (document.Components ??= new()).SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes!["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        return Task.CompletedTask;
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = builder.Environment.IsProduction();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = ApplicationClaims.Role,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenOptions!.JwtIssuer,
        ValidAudience = tokenOptions!.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions!.JwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
    {
        throw new NotFoundException();
    }
    else if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
    {
        throw new MethodNotAllowedException();

    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddPreferredSecuritySchemes("Bearer");
    });
}

app.MapControllers();

app.Run();