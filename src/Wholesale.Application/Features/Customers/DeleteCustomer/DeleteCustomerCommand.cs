using MediatR;

namespace Wholesale.Application.Features.Customers.DeleteCustomer;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest;
