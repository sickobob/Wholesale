using FluentValidation;

namespace Wholesale.Application.Features.Auth.Login;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Login).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
