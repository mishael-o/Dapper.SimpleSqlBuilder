namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the update builder type.
/// </summary>
public interface IUpdateBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the UPDATE clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IUpdateBuilder"/> instance.</returns>
    IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the UPDATE clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IUpdateBuilder"/> instance.</returns>
    IUpdateBuilder Update(FormattableString formattable);

#endif
}
