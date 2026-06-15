using MediatR;
using Wholesale.Application.Features.Customers.Dtos;

namespace Wholesale.Application.Features.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string LegalAddress,
    IReadOnlyList<string> Permissions) : IRequest<CustomerDto>;
