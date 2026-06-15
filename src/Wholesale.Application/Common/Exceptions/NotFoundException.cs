namespace Wholesale.Application.Common.Exceptions;

public class NotFoundException(string entity, Guid id)
    : Exception($"{entity} с id '{id}' не найден.");
