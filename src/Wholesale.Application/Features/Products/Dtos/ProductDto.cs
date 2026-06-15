namespace Wholesale.Application.Features.Products.Dtos;

public sealed record ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public bool IsAvailable { get; init; }
}
