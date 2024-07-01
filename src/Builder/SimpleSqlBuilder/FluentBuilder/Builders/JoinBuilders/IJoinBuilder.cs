namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the join builder type.
/// </summary>
public interface IJoinBuilder : IWhereBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>INNER JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder InnerJoin([InterpolatedStringHandlerArgument("")] ref InnerJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>INNER JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref InnerJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>LEFT JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder LeftJoin([InterpolatedStringHandlerArgument("")] ref LeftJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>LEFT JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref LeftJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>RIGHT JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder RightJoin([InterpolatedStringHandlerArgument("")] ref RightJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>RIGHT JOIN</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder RightJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref RightJoinInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>INNER JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder InnerJoin(FormattableString formattable);

    /// <summary>
    /// Appends the <c>INNER JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder InnerJoin(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the <c>LEFT JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder LeftJoin(FormattableString formattable);

    /// <summary>
    /// Appends the <c>LEFT JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder LeftJoin(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the <c>RIGHT JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder RightJoin(FormattableString formattable);

    /// <summary>
    /// Appends the <c>RIGHT JOIN</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/> instance.</returns>
    IJoinBuilder RightJoin(bool condition, FormattableString formattable);

#endif
}
