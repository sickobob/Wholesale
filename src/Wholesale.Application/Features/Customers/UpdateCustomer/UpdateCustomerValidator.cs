using FluentValidation;
using Wholesale.Application.Security;

namespace Wholesale.Application.Features.Customers.UpdateCustomer;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.LegalAddress).NotEmpty().MaximumLength(1024);
        RuleForEach(x => x.Permissions)
            .Must(code => Permissions.All.Contains(code))
            .WithMessage((_, code) => $"Неизвестное право: '{code}'.");
    }
}
