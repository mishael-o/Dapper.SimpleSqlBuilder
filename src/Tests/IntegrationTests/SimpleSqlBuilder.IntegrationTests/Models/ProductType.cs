namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record ProductType
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
}
