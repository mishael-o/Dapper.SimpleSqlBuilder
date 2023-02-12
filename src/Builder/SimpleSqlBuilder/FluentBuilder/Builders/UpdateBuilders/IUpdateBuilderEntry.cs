namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the update builder type.
/// </summary>
public interface IUpdateBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'update' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'update' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IUpdateBuilder"/>.</returns>
    IUpdateBuilder Update(FormattableString formattable);

#endif
}
