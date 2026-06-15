using MediatR;
using Wholesale.Application.Features.Orders.Dtos;

namespace Wholesale.Application.Features.Orders.GetMyOrders;

public sealed record GetMyOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;
