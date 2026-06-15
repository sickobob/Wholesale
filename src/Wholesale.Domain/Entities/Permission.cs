using Wholesale.Domain.Common;

namespace Wholesale.Domain.Entities;

/// <summary>Справочник прав. Заполняется сидированием из констант Permissions.</summary>
public class Permission : AuditableEntity
{
    public required string Code { get; set; }
    public string? Description { get; set; }
}
