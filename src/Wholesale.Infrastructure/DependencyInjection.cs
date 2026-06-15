using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wholesale.Application.Abstractions;
using Wholesale.Infrastructure.Auth;
using Wholesale.Infrastructure.Persistence;
using Wholesale.Infrastructure.Persistence.Interceptors;
using Wholesale.Infrastructure.Persistence.Repositories;
using Wholesale.Infrastructure.Services;

namespace Wholesale.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<AuditInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
            options
                .UseNpgsql(configuration.GetConnectionString("Default"))
                .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.AddScoped<IEmailSender, LoggingEmailSender>();

        return services;
    }
}
