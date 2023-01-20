namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the fluent delete builder entry type or contract.
/// </summary>
public interface IDeleteBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Deletes from the specified table.
    /// </summary>
    /// <param name="handler">The <see cref="DeleteInterpolatedStringHandler"/>.</param>
    /// <returns>Returns <see cref="IDeleteBuilder"/>.</returns>
    IDeleteBuilder DeleteFrom([InterpolatedStringHandlerArgument("")] ref DeleteInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Deletes from the specified table.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>Returns <see cref="IDeleteBuilder"/>.</returns>
    IDeleteBuilder DeleteFrom(FormattableString formattable);

#endif
}
