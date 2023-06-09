namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the fetch builder type.
/// </summary>
public interface IFetchBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the 'fetch next' clause, the <paramref name="rows"/>, and the 'rows only' clause to the builder.
    /// </summary>
    /// <param name="rows">The number of rows to fetch.</param>
    /// <returns>The <see cref="IFluentSqlBuilder"/>.</returns>
    IFluentSqlBuilder FetchNext(int rows);
}
