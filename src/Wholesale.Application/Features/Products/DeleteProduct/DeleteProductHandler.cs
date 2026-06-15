using MediatR;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Features.Products.DeleteProduct;

public sealed class DeleteProductHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await uow.Products.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        uow.Products.Remove(product); // soft-delete
        await uow.SaveChangesAsync(ct);
    }
}
