using Microsoft.AspNetCore.Authorization;

namespace Wholesale.Api.Auth;

/// <summary>
/// Закрывает endpoint конкретным правом: [HasPermission(Permissions.Products.Manage)].
/// Имя policy = код права; политика создаётся динамически в PermissionPolicyProvider.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission);
