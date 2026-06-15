using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Features.Orders.Dtos;

namespace Wholesale.Application.Features.Orders.GetMyOrders;

public sealed class GetMyOrdersHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<GetMyOrdersQuery, IReadOnlyList<OrderDto>>
{
    public async Task<IReadOnlyList<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken ct)
        => await uow.Orders.Query()
            .Where(o => o.CustomerId == currentUser.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
}
