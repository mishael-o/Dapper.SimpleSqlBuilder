﻿namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// An interface that defines the select builder entry type.
/// </summary>
public interface ISelectBuilderEntry
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Appends the 'select' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="ISelectBuilder"/>.</returns>
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    /// <summary>
    /// Appends the 'select distinct' clause and the interpolated string to the builder.
    /// </summary>
    /// <param name="handler">The handler for the interpolated string.</param>
    /// <returns><see cref="ISelectDistinctBuilder"/>.</returns>
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);
#else

    /// <summary>
    /// Appends the 'select' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="ISelectBuilder"/>.</returns>
    ISelectBuilder Select(FormattableString formattable);

    /// <summary>
    /// Appends the 'select distinct' clause and the interpolated string or <see cref="FormattableString"/> to the builder.
    /// </summary>
    /// <param name="formattable">The <see cref="FormattableString"/>.</param>
    /// <returns><see cref="ISelectDistinctBuilder"/>.</returns>
    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

#endif
}
