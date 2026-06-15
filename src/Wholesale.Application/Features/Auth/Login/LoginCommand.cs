using MediatR;

namespace Wholesale.Application.Features.Auth.Login;

public sealed record LoginCommand(string Login, string Password) : IRequest<LoginResponse>;

public sealed record LoginResponse(string AccessToken);
