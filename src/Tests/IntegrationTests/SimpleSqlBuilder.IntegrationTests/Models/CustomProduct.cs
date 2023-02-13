namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record CustomProduct : Product
{
    public new CustomId Id { get; set; }
    public new CustomId? TypeId { get; set; }
}
