using Wholesale.Domain.Common;

namespace Wholesale.Domain.Entities;

public class Product : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}
