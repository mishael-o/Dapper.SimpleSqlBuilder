namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the order by builder type.
/// </summary>
public interface IOrderByBuilder : IOrderByBuilderEntry, IFetchBuilder, IOffsetRowsBuilder, ILimitBuilder
{
}
