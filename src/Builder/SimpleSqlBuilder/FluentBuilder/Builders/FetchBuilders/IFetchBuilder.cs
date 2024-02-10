namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the fetch builder type.
/// </summary>
public interface IFetchBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the FETCH NEXT clause, the <paramref name="rows"/>, and the ROWS ONLY clause to the builder.
    /// </summary>
    /// <param name="rows">The number of rows to fetch.</param>
    /// <returns>The <see cref="IFluentSqlBuilder"/> instance.</returns>
    IFluentSqlBuilder FetchNext(int rows);
}
