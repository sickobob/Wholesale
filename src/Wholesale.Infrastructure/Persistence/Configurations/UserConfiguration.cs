using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wholesale.Domain.Entities;

namespace Wholesale.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Login).HasMaxLength(256).IsRequired();
        builder.Property(u => u.Name).HasMaxLength(256).IsRequired();
        builder.Property(u => u.LegalAddress).HasMaxLength(1024);
        // Уникальность логина среди "живых" записей (soft-delete не блокирует переиспользование почты)
        builder.HasIndex(u => u.Login).IsUnique().HasFilter("\"IsDeleted\" = false");

        builder.HasMany(u => u.Permissions)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId);
    }
}

public sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(up => new { up.UserId, up.PermissionId });

        builder.HasOne(up => up.Permission)
            .WithMany()
            .HasForeignKey(up => up.PermissionId);
    }
}

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(p => p.Code).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => p.Code).IsUnique();
    }
}
