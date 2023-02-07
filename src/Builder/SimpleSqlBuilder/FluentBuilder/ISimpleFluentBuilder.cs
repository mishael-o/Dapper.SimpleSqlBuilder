namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the simple fluent builder type or contract.
/// </summary>
public interface ISimpleFluentBuilder : IDeleteBuilder, IInsertBuilder, ISelectBuilder, ISelectDistinctBuilder, ISelectFromBuilder, IUpdateBuilder, IWhereFilter
{
}
