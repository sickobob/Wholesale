using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Features.Customers.Dtos;
using Wholesale.Domain.Enums;

namespace Wholesale.Application.Features.Customers.GetCustomers;

public sealed class GetCustomersHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCustomersQuery, IReadOnlyList<CustomerDto>>
{
    public async Task<IReadOnlyList<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken ct)
        => await uow.Users.Query()
            .Where(u => u.Role == UserRole.Customer)
            .OrderBy(u => u.Name)
            .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
}
