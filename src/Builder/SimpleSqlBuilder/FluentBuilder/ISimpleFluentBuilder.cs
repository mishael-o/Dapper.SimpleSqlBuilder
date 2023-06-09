namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the simple fluent builder type.
/// </summary>
public interface ISimpleFluentBuilder :
    IDeleteBuilder,
    IInsertBuilder,
    ISelectBuilder,
    ISelectDistinctBuilder,
    ISelectFromBuilder,
    IUpdateBuilder,
    IWhereFilterBuilder,
    IOrderByBuilder,
    IFetchBuilder,
    IOffsetRowsBuilder,
    ILimitBuilder,
    IOffsetBuilder
{
}
