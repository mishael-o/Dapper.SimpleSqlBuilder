namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the update builder type.
/// </summary>
public interface IUpdateBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>UPDATE</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IUpdateBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>Update</c> and <c>InsertInto</c> on the same builder instance.</exception>
    IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>UPDATE</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IUpdateBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>Update</c> and <c>InsertInto</c> on the same builder instance.</exception>
    IUpdateBuilder Update(FormattableString formattable);

#endif
}
