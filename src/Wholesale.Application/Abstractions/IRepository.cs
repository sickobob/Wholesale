using Wholesale.Domain.Common;

namespace Wholesale.Application.Abstractions;

/// <summary>
/// Generic-репозиторий. Один EfRepository&lt;T&gt; закрывает все сущности —
/// специфичные запросы строятся через Query() в хендлерах.
/// </summary>
public interface IRepository<T> where T : AuditableEntity
{
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    void Add(T entity);
    void Update(T entity);
    /// <summary>Физического удаления не происходит — AuditInterceptor превращает его в soft-delete.</summary>
    void Remove(T entity);
}
