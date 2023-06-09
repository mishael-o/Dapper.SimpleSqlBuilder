namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the offset rows builder type.
/// </summary>
public interface IOffsetRowsBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the 'offset' clause, the <paramref name="offset"/>, and the and 'rows' clause to the builder.
    /// </summary>
    /// <param name="offset">The number of rows to skip.</param>
    /// <returns>The <see cref="IFetchBuilder"/>.</returns>
    IFetchBuilder OffsetRows(int offset);
}
