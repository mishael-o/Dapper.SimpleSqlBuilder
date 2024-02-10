namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where builder type.
/// </summary>
public interface IWhereBuilder : IWhereBuilderEntry, IGroupByBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the OR clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the OR clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereOrInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the OR clause, starts the filter group, and appends the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder OrWhereFilter([InterpolatedStringHandlerArgument("")] ref WhereOrFilterInterpolatedStringHandler handler);

#else

    /// <summary>
    /// Appends the OR clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(FormattableString formattable);

    /// <summary>
    /// Appends the OR clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the OR clause, starts the filter group, and appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder OrWhereFilter(FormattableString formattable);

#endif

    /// <summary>
    /// Starts the OR clause filter group.
    /// </summary>
    /// <returns>The <see cref="IWhereFilterBuilderEntry"/> instance.</returns>
    IWhereFilterBuilderEntry OrWhereFilter();
}
