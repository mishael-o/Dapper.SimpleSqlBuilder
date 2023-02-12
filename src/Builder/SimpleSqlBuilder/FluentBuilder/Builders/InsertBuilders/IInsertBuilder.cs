namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the insert builder type.
/// </summary>
public interface IInsertBuilder : IInsertValueBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the interpolated string column value to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IInsertBuilder"/>.</returns>
    IInsertBuilder Columns([InterpolatedStringHandlerArgument("")] ref InsertColumnInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the interpolated string or <see cref="FormattableString"/> column value to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IInsertBuilder"/>.</returns>
    IInsertBuilder Columns(FormattableString formattable);

#endif
}
