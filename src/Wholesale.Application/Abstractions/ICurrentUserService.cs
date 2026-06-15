namespace Wholesale.Application.Abstractions;

/// <summary>Текущий пользователь из JWT. Реализация в Api читает HttpContext.</summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}
