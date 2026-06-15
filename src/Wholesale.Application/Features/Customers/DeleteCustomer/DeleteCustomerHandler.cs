using MediatR;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;

namespace Wholesale.Application.Features.Customers.DeleteCustomer;

public sealed class DeleteCustomerHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand request, CancellationToken ct)
    {
        var user = await uow.Users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        uow.Users.Remove(user); // -> soft-delete через AuditInterceptor
        await uow.SaveChangesAsync(ct);
    }
}
