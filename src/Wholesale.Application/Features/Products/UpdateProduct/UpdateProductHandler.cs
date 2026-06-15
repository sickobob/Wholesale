using AutoMapper;
using MediatR;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Application.Features.Products.Dtos;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await uow.Products.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.IsAvailable = request.IsAvailable;

        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProductDto>(product);
    }
}
