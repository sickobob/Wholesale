using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;

namespace Wholesale.Application.Features.Auth.Login;

public sealed class LoginHandler(
    IUnitOfWork uow,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await uow.Users.Query()
            .Include(u => u.Permissions)
            .ThenInclude(up => up.Permission)
            .FirstOrDefaultAsync(u => u.Login == request.Login.ToLowerInvariant(), ct);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Неверный логин или пароль.");

        var permissions = user.Permissions
            .Select(p => p.Permission!.Code)
            .ToArray();

        return new LoginResponse(jwtTokenService.CreateToken(user, permissions));
    }
}
