namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

public record Product
{
    public Guid Id { get; init; }
    public Guid TypeId { get; init; }
    public string? Tag { get; init; }
    public string Description { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
}