using Wholesale.Domain.Enums;

namespace Wholesale.Application.Features.Orders.Dtos;

public sealed record OrderDto
{
    public Guid Id { get; init; }
    public OrderStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
}

public sealed record OrderItemDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Total { get; init; }
}
