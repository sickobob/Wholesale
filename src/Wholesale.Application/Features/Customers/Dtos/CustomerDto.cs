namespace Wholesale.Application.Features.Customers.Dtos;

public sealed record CustomerDto
{
    public Guid Id { get; init; }
    public string Login { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? LegalAddress { get; init; }
    public IReadOnlyList<string> Permissions { get; init; } = [];
    public DateTime CreatedAt { get; init; }
}
