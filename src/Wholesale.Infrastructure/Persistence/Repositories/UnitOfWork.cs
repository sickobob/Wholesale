using Wholesale.Application.Abstractions;
using Wholesale.Domain.Entities;

namespace Wholesale.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IRepository<User> Users { get; } = new EfRepository<User>(context);
    public IRepository<Permission> Permissions { get; } = new EfRepository<Permission>(context);
    public IRepository<Product> Products { get; } = new EfRepository<Product>(context);
    public IRepository<Order> Orders { get; } = new EfRepository<Order>(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
