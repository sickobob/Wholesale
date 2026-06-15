using FluentValidation;
using Wholesale.Application.Security;

namespace Wholesale.Application.Features.Customers.RegisterCustomer;

public sealed class RegisterCustomerValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerValidator()
    {
        RuleFor(x => x.Login).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.LegalAddress).NotEmpty().MaximumLength(1024);
        RuleForEach(x => x.Permissions)
            .Must(code => Permissions.All.Contains(code))
            .WithMessage((_, code) => $"Неизвестное право: '{code}'.");
    }
}
