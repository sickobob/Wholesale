using MediatR;
using Wholesale.Application.Features.Orders.Dtos;

namespace Wholesale.Application.Features.Orders.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest<OrderDto>;
