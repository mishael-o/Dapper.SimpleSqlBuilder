namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the offset rows builder type.
/// </summary>
public interface IOffsetRowsBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the <c>OFFSET</c> clause, the <paramref name="offset"/>, and the <c>ROWS</c> clause to the builder.
    /// </summary>
    /// <param name="offset">The number of rows to skip.</param>
    /// <returns>The <see cref="IFetchBuilder"/> instance.</returns>
    IFetchBuilder OffsetRows(int offset);
}
