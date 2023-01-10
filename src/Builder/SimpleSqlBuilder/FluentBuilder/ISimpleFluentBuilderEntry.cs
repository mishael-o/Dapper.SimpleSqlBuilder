namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the simple fluent builder entry type or contract.
/// </summary>
public interface ISimpleFluentBuilderEntry : IDeleteBuilderEntry, IInsertBuilderEntry, ISelectBuilderEntry, IUpdateBuilderEntry, IFluentBuilder
{
}
