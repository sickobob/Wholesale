using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Application.Features.Orders.Dtos;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Features.Orders.CreateOrder;

public sealed class CreateOrderHandler(
    IUnitOfWork uow,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var productIds = request.Items.Select(i => i.ProductId).ToArray();
        var products = await uow.Products.Query()
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, ct);

        var order = new Order { CustomerId = currentUser.UserId };

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new NotFoundException(nameof(Product), item.ProductId);

            order.AddItem(product, item.Quantity); // проверка доступности + снапшот цены в домене
        }

        uow.Orders.Add(order);
        await uow.SaveChangesAsync(ct);

        return mapper.Map<OrderDto>(order);
    }
}
