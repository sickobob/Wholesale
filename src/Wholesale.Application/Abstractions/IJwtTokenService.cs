using Wholesale.Domain.Entities;

namespace Wholesale.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateToken(User user, IReadOnlyCollection<string> permissionCodes);
}
