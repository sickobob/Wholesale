using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Security;
using Wholesale.Domain.Entities;
using Wholesale.Domain.Enums;

namespace Wholesale.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await context.Database.MigrateAsync();

        // 1. Справочник прав
        var existingCodes = await context.Permissions.Select(p => p.Code).ToListAsync();
        var missing = Permissions.All.Except(existingCodes)
            .Select(code => new Permission { Code = code })
            .ToList();
        if (missing.Count != 0)
        {
            context.Permissions.AddRange(missing);
            await context.SaveChangesAsync();
        }

        // 2. Администратор
        var adminLogin = config["Seed:AdminLogin"] ?? "admin@wholesale.local";
        if (!await context.Users.AnyAsync(u => u.Login == adminLogin))
        {
            var admin = new User
            {
                Login = adminLogin,
                Name = "Администратор",
                PasswordHash = hasher.Hash(config["Seed:AdminPassword"] ?? "Admin123!"),
                Role = UserRole.Administrator
            };
            var allPermissions = await context.Permissions.ToListAsync();
            admin.ReplacePermissions(allPermissions);

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
