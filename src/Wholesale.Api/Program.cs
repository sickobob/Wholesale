using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Wholesale.Api.Auth;
using Wholesale.Api.Middleware;
using Wholesale.Api.Services;
using Wholesale.Application;
using Wholesale.Application.Abstractions;
using Wholesale.Infrastructure;
using Wholesale.Infrastructure.Auth;
using Wholesale.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Слои
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Текущий пользователь
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Аутентификация (JWT)
var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey))
        };
    });

// Авторизация по правам
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // /openapi/v1.json — типизированная схема для кодогенерации

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference(); // UI документации: /scalar/v1

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Миграции + сидирование (права, админ)
await DbSeeder.SeedAsync(app.Services);

app.Run();
