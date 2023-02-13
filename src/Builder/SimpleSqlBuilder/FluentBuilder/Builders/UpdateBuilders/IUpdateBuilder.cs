namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the update builder type.
/// </summary>
public interface IUpdateBuilder : IWhereBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'set' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Set([InterpolatedStringHandlerArgument("")] ref UpdateSetInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'set' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Set(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref UpdateSetInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'set' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Set(FormattableString formattable);

    /// <summary>
    /// Appends the 'set' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="condition">The value to determine whether the method should be executed.</param>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Set(bool condition, FormattableString formattable);

#endif
}
