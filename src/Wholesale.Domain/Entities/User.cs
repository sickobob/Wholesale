using Wholesale.Domain.Common;
using Wholesale.Domain.Enums;

namespace Wholesale.Domain.Entities;

public class User : AuditableEntity
{
    public required string Login { get; set; }          // email
    public required string Name { get; set; }
    public string? LegalAddress { get; set; }           // юр. адрес (для заказчиков)
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public List<UserPermission> Permissions { get; set; } = [];

    public void ReplacePermissions(IEnumerable<Permission> permissions)
    {
        Permissions.Clear();
        Permissions.AddRange(permissions.Select(p => new UserPermission
        {
            UserId = Id,
            PermissionId = p.Id
        }));
    }
}
