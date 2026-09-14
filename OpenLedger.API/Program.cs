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
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

#region Options
builder.Services.AddOptionsWithValidateOnStart<TokenOptions>().BindConfiguration(TokenOptions.SectionName).ValidateDataAnnotations();
builder.Services.AddOptionsWithValidateOnStart<EmailOptions>().BindConfiguration(EmailOptions.SectionName).ValidateDataAnnotations();
builder.Services.AddOptionsWithValidateOnStart<DbOptions>().BindConfiguration(DbOptions.SectionName).ValidateDataAnnotations();

var tokenOptions = builder.Configuration.GetSection(TokenOptions.SectionName).Get<TokenOptions>();
#endregion

builder.Services.AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddHttpContextAccessor()
                .AddControllers();

builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.UseRuntimeCompilation();
    opts.Discovery.IncludeAssembly(typeof(OpenLedger.Application.DependencyInjection).Assembly);
    opts.UseEntityFrameworkCoreTransactions();
});

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

#region Token
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.JwtSecret!)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

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
#endregion

var app = builder.Build();

app.UseHttpsRedirection()
   .UseAuthentication()
   .UseAuthorization();

#region Exception
app.UseMiddleware<GlobalExceptionMiddleware>()
   .UseStatusCodePages(async context =>
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
#endregion

#region Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddPreferredSecuritySchemes("Bearer");
    });
}
#endregion

app.MapControllers();

app.Run();