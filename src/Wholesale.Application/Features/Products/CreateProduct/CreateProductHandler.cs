using AutoMapper;
using MediatR;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Features.Products.Dtos;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Features.Products.CreateProduct;

public sealed class CreateProductHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = mapper.Map<Product>(request);
        uow.Products.Add(product);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProductDto>(product);
    }
}
