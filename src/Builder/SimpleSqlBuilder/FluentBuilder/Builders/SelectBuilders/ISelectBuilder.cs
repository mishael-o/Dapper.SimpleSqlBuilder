namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the select builder type.
/// </summary>
public interface ISelectBuilder : ISelectFromBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the SELECT clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="ISelectBuilder"/> instance.</returns>
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

#else

    /// <summary>
    /// Appends the SELECT clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectBuilder"/> instance.</returns>
    ISelectBuilder Select(FormattableString formattable);

#endif
}
