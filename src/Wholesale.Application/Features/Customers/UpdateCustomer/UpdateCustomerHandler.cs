using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Application.Features.Customers.Dtos;

namespace Wholesale.Application.Features.Customers.UpdateCustomer;

public sealed class UpdateCustomerHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken ct)
    {
        var user = await uow.Users.Query()
            .Include(u => u.Permissions)
            .ThenInclude(up => up.Permission)
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.User), request.Id);

        user.Name = request.Name;
        user.LegalAddress = request.LegalAddress;

        var permissions = await uow.Permissions.Query()
            .Where(p => request.Permissions.Contains(p.Code))
            .ToListAsync(ct);
        user.ReplacePermissions(permissions);

        await uow.SaveChangesAsync(ct);
        return mapper.Map<CustomerDto>(user);
    }
}
