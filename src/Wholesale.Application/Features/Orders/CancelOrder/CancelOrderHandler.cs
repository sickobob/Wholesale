using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Application.Features.Orders.Dtos;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Features.Orders.CancelOrder;

public sealed class CancelOrderHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<CancelOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CancelOrderCommand request, CancellationToken ct)
    {
        // Заказчик может отменить только СВОЙ заказ — фильтр по CustomerId.
        var order = await uow.Orders.Query()
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.CustomerId == currentUser.UserId, ct)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        order.Cancel(); // DomainException, если уже оплачен
        await uow.SaveChangesAsync(ct);

        return mapper.Map<OrderDto>(order);
    }
}
