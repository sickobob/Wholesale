using MediatR;
using Wholesale.Application.Features.Products.Dtos;

namespace Wholesale.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    bool IsAvailable) : IRequest<ProductDto>;
