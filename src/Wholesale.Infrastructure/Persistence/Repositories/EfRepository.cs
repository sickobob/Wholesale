using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Domain.Common;

namespace Wholesale.Infrastructure.Persistence.Repositories;

public sealed class EfRepository<T>(AppDbContext context) : IRepository<T>
    where T : AuditableEntity
{
    private readonly DbSet<T> _set = context.Set<T>();

    public IQueryable<T> Query() => _set.AsQueryable();

    public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _set.FirstOrDefaultAsync(e => e.Id == id, ct);

    public void Add(T entity) => _set.Add(entity);

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity); // -> soft-delete (AuditInterceptor)
}
