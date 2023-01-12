namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

public record ProductType
{
    public Guid Id { get; set; }
    public string Description { get; set; } = default!;
}
