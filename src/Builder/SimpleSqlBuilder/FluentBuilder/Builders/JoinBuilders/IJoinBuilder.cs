namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the join builder type.
/// </summary>
public interface IJoinBuilder : IWhereBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'inner join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder InnerJoin([InterpolatedStringHandlerArgument("")] ref InnerJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'inner join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref InnerJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'left join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder LeftJoin([InterpolatedStringHandlerArgument("")] ref LeftJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'left join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref LeftJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'right join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder RightJoin([InterpolatedStringHandlerArgument("")] ref RightJoinInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'right join' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder RightJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref RightJoinInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'inner join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder InnerJoin(FormattableString formattable);

    /// <summary>
    /// Appends the 'inner join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder InnerJoin(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the 'left join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder LeftJoin(FormattableString formattable);

    /// <summary>
    /// Appends the 'left join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder LeftJoin(bool condition, FormattableString formattable);

    /// <summary>
    /// Appends the 'right join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder RightJoin(FormattableString formattable);

    /// <summary>
    /// Appends the 'right join' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IJoinBuilder"/>.</returns>
    IJoinBuilder RightJoin(bool condition, FormattableString formattable);

#endif
}
