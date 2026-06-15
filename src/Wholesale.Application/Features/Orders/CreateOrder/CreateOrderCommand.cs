using MediatR;
using Wholesale.Application.Features.Orders.Dtos;

namespace Wholesale.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderCommand(IReadOnlyList<CreateOrderItem> Items) : IRequest<OrderDto>;

public sealed record CreateOrderItem(Guid ProductId, int Quantity);
