using Microsoft.AspNetCore.Authorization;

namespace Wholesale.Api.Auth;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
