using MediatR;

namespace Wholesale.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : IRequest;
