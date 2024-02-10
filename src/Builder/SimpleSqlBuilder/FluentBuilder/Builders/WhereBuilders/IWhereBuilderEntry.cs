namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where builder type.
/// </summary>
public interface IWhereBuilderEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present) and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present) and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present), starts the filter group, and appends the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present) and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(FormattableString formattable);

    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present) and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the WHERE clause (or the AND clause, if a WHERE clause is present), starts the filter group, and appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WhereFilter(FormattableString formattable);

#endif

    /// <summary>
    /// Starts the WHERE clause (or the AND clause, if a WHERE clause is present) filter group.
    /// </summary>
    /// <returns>The <see cref="IWhereFilterBuilderEntry"/> instance.</returns>
    IWhereFilterBuilderEntry WhereFilter();
}
