using Dapper.SimpleSqlBuilder.IntegrationTests.Common;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public record CustomProductType : ProductType
{
    public new CustomId Id { get; set; }
}
