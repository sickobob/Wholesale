using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Wholesale.Api.Auth;

/// <summary>
/// Динамически создаёт policy для любого кода права —
/// не нужно регистрировать каждую политику вручную в AddAuthorization.
/// </summary>
public sealed class PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy is not null)
            return policy;

        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}
