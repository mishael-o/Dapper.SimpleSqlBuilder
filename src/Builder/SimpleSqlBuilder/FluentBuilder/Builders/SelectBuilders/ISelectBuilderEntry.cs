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
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the <c>SELECT DISTINCT</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="ISelectDistinctBuilder"/> instance.</returns>
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>SELECT</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectBuilder"/> instance.</returns>
    ISelectBuilder Select(FormattableString formattable);

    /// <summary>
    /// Appends the <c>SELECT DISTINCT</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectDistinctBuilder"/> instance.</returns>
    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

#endif
}
