using Dapper.SimpleSqlBuilder.IntegrationTests.Common;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public record CustomProduct : Product
{
    public new CustomId Id { get; init; }
    public new CustomId TypeId { get; init; }
}