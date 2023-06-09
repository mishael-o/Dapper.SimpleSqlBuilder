namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the limit builder type.
/// </summary>
public interface ILimitBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the 'limit' clause and the <paramref name="rows"/> to the builder.
    /// </summary>
    /// <param name="rows">The number of rows to fetch.</param>
    /// <returns>The <see cref="IFetchBuilder"/>.</returns>
    IOffsetBuilder Limit(int rows);
}
