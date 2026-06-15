using MediatR;
using Wholesale.Application.Features.Products.Dtos;

namespace Wholesale.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    bool IsAvailable) : IRequest<ProductDto>;
