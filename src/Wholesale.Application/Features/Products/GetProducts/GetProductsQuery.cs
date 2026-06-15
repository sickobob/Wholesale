using MediatR;
using Wholesale.Application.Features.Products.Dtos;

namespace Wholesale.Application.Features.Products.GetProducts;

public sealed record GetProductsQuery(bool OnlyAvailable = false) : IRequest<IReadOnlyList<ProductDto>>;
