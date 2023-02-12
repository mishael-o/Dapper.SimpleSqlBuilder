namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the insert builder entry type.
/// </summary>
public interface IInsertBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'insert into' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="IInsertBuilder"/>.</returns>
    IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'insert into' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="IInsertBuilder"/>.</returns>
    IInsertBuilder InsertInto(FormattableString formattable);

#endif
}
