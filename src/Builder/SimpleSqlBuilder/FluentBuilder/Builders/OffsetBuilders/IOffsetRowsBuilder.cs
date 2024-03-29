﻿namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the offset rows builder type.
/// </summary>
public interface IOffsetRowsBuilder : IFluentSqlBuilder
{
    /// <summary>
    /// Appends the OFFSET clause, the <paramref name="offset"/>, and the ROWS clause to the builder.
    /// </summary>
    /// <param name="offset">The number of rows to skip.</param>
    /// <returns>The <see cref="IFetchBuilder"/> instance.</returns>
    IFetchBuilder OffsetRows(int offset);
}
