namespace Wholesale.Domain.Entities;

/// <summary>Связь пользователь-право (составной ключ, без аудита — чистая m2m-связка).</summary>
public class UserPermission
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
