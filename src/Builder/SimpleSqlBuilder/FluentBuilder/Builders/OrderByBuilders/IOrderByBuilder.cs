namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the order by builder type.
/// </summary>
public interface IOrderByBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'order by' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IOrderByBuilder"/>.</returns>
    IOrderByBuilder OrderBy([InterpolatedStringHandlerArgument("")] ref OrderByInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'order by' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IOrderByBuilder"/>.</returns>
    IOrderByBuilder OrderBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref OrderByInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'order by' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IOrderByBuilder"/>.</returns>
    IOrderByBuilder OrderBy(FormattableString formattable);

    /// <summary>
    /// Appends the 'order by' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IOrderByBuilder"/>.</returns>
    IOrderByBuilder OrderBy(bool condition, FormattableString formattable);

#endif
}
