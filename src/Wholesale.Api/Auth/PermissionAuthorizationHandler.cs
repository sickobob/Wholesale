using Microsoft.AspNetCore.Authorization;
using Wholesale.Application.Security;

namespace Wholesale.Api.Auth;

/// <summary>Проверяет наличие claim'а "permission" с нужным кодом в JWT.</summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(Permissions.ClaimType, requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
