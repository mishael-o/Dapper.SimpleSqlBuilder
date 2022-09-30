namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

public record ProductType
{
    public Guid Id { get; init; }
    public string Description { get; init; } = default!;
}