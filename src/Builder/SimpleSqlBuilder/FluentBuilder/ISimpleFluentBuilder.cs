namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// An interface that defines the fluent builder type or contract.
/// </summary>
public interface ISimpleFluentBuilder : IDeleteBuilderEntry, IDeleteBuilder, IInsertBuilderEntry, IInsertBuilder, ISelectBuilderEntry, ISelectBuilder, ISelectDistinctBuilder, ISelectFromBuilder, IWhereFilter
{
}
