namespace Wholesale.Domain.Exceptions;

/// <summary>Нарушение бизнес-правила. Транслируется в HTTP 409.</summary>
public class DomainException(string message) : Exception(message);
