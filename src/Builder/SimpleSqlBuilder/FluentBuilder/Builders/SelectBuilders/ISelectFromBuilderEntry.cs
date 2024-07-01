namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the select from builder entry type.
/// </summary>
public interface ISelectFromBuilderEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the <c>FROM</c> clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="ISelectFromBuilder"/> instance.</returns>
    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the <c>FROM</c> clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="ISelectFromBuilder"/> instance.</returns>
    ISelectFromBuilder From(FormattableString formattable);

#endif
}
