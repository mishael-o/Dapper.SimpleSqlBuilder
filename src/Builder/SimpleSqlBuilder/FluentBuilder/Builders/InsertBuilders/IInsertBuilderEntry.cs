namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the insert builder entry type.
/// </summary>
public interface IInsertBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>INSERT INTO</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IInsertBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>InsertInto</c> and <c>DeleteFrom</c> on the same builder instance.</exception>
    IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>INSERT INTO</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IInsertBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>InsertInto</c> and <c>DeleteFrom</c> on the same builder instance.</exception>
    IInsertBuilder InsertInto(FormattableString formattable);

#endif
}
