namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the delete builder entry type.
/// </summary>
public interface IDeleteBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'delete from' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IDeleteBuilder"/>.</returns>
    IDeleteBuilder DeleteFrom([InterpolatedStringHandlerArgument("")] ref DeleteInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'delete from' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IDeleteBuilder"/>.</returns>
    IDeleteBuilder DeleteFrom(FormattableString formattable);

#endif
}
