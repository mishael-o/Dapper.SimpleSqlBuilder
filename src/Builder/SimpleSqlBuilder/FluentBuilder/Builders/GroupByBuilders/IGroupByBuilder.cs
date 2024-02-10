namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the group by builder type.
/// </summary>
public interface IGroupByBuilder : IHavingBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the GROUP BY clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IGroupByBuilder"/> instance.</returns>
    IGroupByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the GROUP BY clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IGroupByBuilder"/> instance.</returns>
    IGroupByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref GroupByInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the GROUP BY clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IGroupByBuilder"/> instance.</returns>
    IGroupByBuilder GroupBy(FormattableString formattable);

    /// <summary>
    /// Appends the GROUP BY clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IGroupByBuilder"/> instance.</returns>
    IGroupByBuilder GroupBy(bool condition, FormattableString formattable);

#endif
}
