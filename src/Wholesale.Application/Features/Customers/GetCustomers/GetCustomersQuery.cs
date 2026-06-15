using MediatR;
using Wholesale.Application.Features.Customers.Dtos;

namespace Wholesale.Application.Features.Customers.GetCustomers;

public sealed record GetCustomersQuery : IRequest<IReadOnlyList<CustomerDto>>;
