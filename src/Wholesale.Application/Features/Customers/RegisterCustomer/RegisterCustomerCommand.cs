using MediatR;
using Wholesale.Application.Features.Customers.Dtos;

namespace Wholesale.Application.Features.Customers.RegisterCustomer;

/// <param name="Permissions">null — выдать стандартный набор; иначе — скорректированный список кодов.</param>
public sealed record RegisterCustomerCommand(
    string Login,
    string Name,
    string LegalAddress,
    IReadOnlyList<string>? Permissions) : IRequest<CustomerDto>;
