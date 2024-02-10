namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the offset builder type.
/// </summary>
public interface IOffsetBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the OFFSET clause and the <paramref name="offset"/> to the builder.
    /// </summary>
    /// <param name="offset">The number of rows to skip.</param>
    /// <returns>The <see cref="IFetchBuilder"/> instance.</returns>
    IFluentSqlBuilder Offset(int offset);
}
