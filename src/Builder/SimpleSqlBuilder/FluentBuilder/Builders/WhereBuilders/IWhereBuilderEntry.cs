namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where builder type.
/// </summary>
public interface IWhereBuilderEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present) and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IWhereBuilder"/>.</returns>
    IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present) and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IWhereBuilder"/>.</returns>
    IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present), starts the filter group, and appends the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present) and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IWhereBuilder"/>.</returns>
    IWhereBuilder Where(FormattableString formattable);

    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present) and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IWhereBuilder"/>.</returns>
    IWhereBuilder Where(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the 'where' clause (or the 'and' clause, if a 'where' clause is present), starts the filter group, and appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IWhereFilterBuilder"/>.</returns>
    IWhereFilterBuilder WhereFilter(FormattableString formattable);

#endif

    /// <summary>
    /// Starts the 'where' clause (or the 'and' clause, if a 'where' clause is present) filter group.
    /// </summary>
    /// <returns><see cref="IWhereFilterBuilderEntry"/>.</returns>
    IWhereFilterBuilderEntry WhereFilter();
}
