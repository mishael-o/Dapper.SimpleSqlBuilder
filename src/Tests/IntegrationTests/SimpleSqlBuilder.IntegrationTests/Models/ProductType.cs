namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record ProductType
{
    public Guid Id { get; set; }
    public string Description { get; set; } = default!;
}
