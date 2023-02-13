namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record CustomProductType : ProductType
{
    public new CustomId Id { get; set; }
}
