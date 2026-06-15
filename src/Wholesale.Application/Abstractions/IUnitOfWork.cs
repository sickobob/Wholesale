using Wholesale.Domain.Entities;

namespace Wholesale.Application.Abstractions;

/// <summary>Единый Unit-of-Work со всеми репозиториями (как разрешено заданием).</summary>
public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Permission> Permissions { get; }
    IRepository<Product> Products { get; }
    IRepository<Order> Orders { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
