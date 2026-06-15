using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wholesale.Application.Abstractions;
using Wholesale.Domain.Common;

namespace Wholesale.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Автоматически заполняет поля аудита и превращает физическое удаление в soft-delete.
/// Работает для ЛЮБОЙ сущности-наследника AuditableEntity — новые сущности
/// получают аудит "бесплатно".
/// </summary>
public sealed class AuditInterceptor(ICurrentUserService currentUser, TimeProvider time)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null) return;

        var now = time.GetUtcNow().UtcDateTime;
        var userId = currentUser.IsAuthenticated ? currentUser.UserId : Guid.Empty; // Guid.Empty = system

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = now;
                    entry.Entity.ModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified; // delete -> update
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = userId;
                    break;
            }
        }
    }
}
