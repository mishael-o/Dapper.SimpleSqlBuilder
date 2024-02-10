namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the insert value builder type.
/// </summary>
public interface IInsertValueBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the VALUES clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns>The <see cref="IInsertValueBuilder"/> instance.</returns>
    IInsertValueBuilder Values([InterpolatedStringHandlerArgument("")] ref InsertValueInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the VALUES clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns>The <see cref="IInsertValueBuilder"/> instance.</returns>
    IInsertValueBuilder Values(FormattableString formattable);

#endif
}
