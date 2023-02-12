namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where filter builder entry type.
/// </summary>
public interface IWhereFilterBuilderEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'and' clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'and' clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'and' clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WithFilter(FormattableString formattable);

    /// <summary>
    /// Appends 'and' clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WithFilter(bool condition, FormattableString formattable);

#endif
}
