using Dapper.SimpleSqlBuilder.IntegrationTests.Common;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public record CustomProduct : Product
{
    public new CustomId Id { get; set; }
    public new CustomId TypeId { get; set; }
}
