namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the having builder type.
/// </summary>
public interface IHavingBuilder : IOrderByBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'having' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IHavingBuilder"/>.</returns>
    IHavingBuilder Having([InterpolatedStringHandlerArgument("")] ref HavingInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'having' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IHavingBuilder"/>.</returns>
    IHavingBuilder Having(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref HavingInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'having' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IHavingBuilder"/>.</returns>
    IHavingBuilder Having(FormattableString formattable);

    /// <summary>
    /// Appends the 'having' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IHavingBuilder"/>.</returns>
    IHavingBuilder Having(bool condition, FormattableString formattable);

#endif
}
