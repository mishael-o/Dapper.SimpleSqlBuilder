namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the where filter builder type.
/// </summary>
public interface IWhereFilterBuilder : IWhereBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>AND</c> clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>AND</c> clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>OR</c> clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithOrFilter([InterpolatedStringHandlerArgument("")] ref WhereWithOrFilterInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>OR</c> clause filter and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithOrFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithOrFilterInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>AND</c> clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithFilter(FormattableString formattable);

    /// <summary>
    /// Appends <c>AND</c> clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithFilter(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the <c>OR</c> clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithOrFilter(FormattableString formattable);

    /// <summary>
    /// Appends the <c>OR</c> clause filter and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IWhereFilterBuilder"/> instance.</returns>
    IWhereFilterBuilder WithOrFilter(bool condition, FormattableString formattable);

#endif
}
