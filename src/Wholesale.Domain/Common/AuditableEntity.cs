namespace Wholesale.Domain.Common;

/// <summary>
/// Базовый класс всех сущностей: Id, аудит и soft-delete.
/// Поля аудита заполняются автоматически (AuditInterceptor в Infrastructure).
/// </summary>
public abstract class AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
