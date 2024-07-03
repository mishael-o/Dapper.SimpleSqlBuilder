namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the select builder entry type.
/// </summary>
public interface ISelectBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>SELECT</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="ISelectBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>Select</c> and <c>SelectDistinct</c> on the same builder instance.</exception>
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>SELECT DISTINCT</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="ISelectDistinctBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>SelectDistinct</c> and <c>Select</c> on the same builder instance.</exception>
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>SELECT</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>Select</c> and <c>SelectDistinct</c> on the same builder instance.</exception>
    ISelectBuilder Select(FormattableString formattable);

    /// <summary>
    /// Appends the <c>SELECT DISTINCT</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectDistinctBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when two entry clauses are called on the same instance, e.g., calling <c>SelectDistinct</c> and <c>Select</c> on the same builder instance.</exception>
    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

#endif
}
