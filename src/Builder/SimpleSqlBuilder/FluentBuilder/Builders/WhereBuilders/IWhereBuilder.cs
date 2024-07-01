namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where builder type.
/// </summary>
public interface IWhereBuilder : IGroupByBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>WHERE</c> clause and the interpolated string to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>WHERE</c> clause and the interpolated string to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>WHERE</c> clause, starts a filter group, and appends the interpolated string to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>OR</c> clause and the interpolated string to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>OR</c> clause and the interpolated string to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereOrInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>OR</c> clause, starts a filter group, and appends the interpolated string to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder OrWhereFilter([InterpolatedStringHandlerArgument("")] ref WhereOrFilterInterpolatedStringHandler handler);

#else

    /// <summary>
    /// Appends the <c>WHERE</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(FormattableString formattable);

    /// <summary>
    /// Appends the <c>WHERE</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder Where(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the <c>WHERE</c> clause, starts a filter group, and appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WhereFilter(FormattableString formattable);

    /// <summary>
    /// Appends the <c>OR</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(FormattableString formattable);

    /// <summary>
    /// Appends the <c>OR</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereBuilder"/> instance.</returns>
    IWhereBuilder OrWhere(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the <c>OR</c> clause, starts a filter group, and appends the interpolated string or <see cref="FormattableString"/> to the builder.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder OrWhereFilter(FormattableString formattable);

#endif

    /// <summary>
    /// Starts a <c>WHERE</c> clause filter group.
    /// If the <c>WHERE</c> clause is already present, the <c>AND</c> clause is appended.
    /// </summary>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WhereFilter();

    /// <summary>
    /// Starts an <c>OR</c> clause filter group.
    /// If no <c>WHERE</c> clause is present, the <c>WHERE</c> clause is appended.
    /// </summary>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder OrWhereFilter();
}
