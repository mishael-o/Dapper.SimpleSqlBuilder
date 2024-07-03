#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// A handler used by the language compiler to append interpolated strings into <see cref="IFluentBuilder"/> instances.
/// </summary>
[InterpolatedStringHandler]
public ref struct GroupByInterpolatedStringHandler
{
    private readonly IFluentBuilderFormatter? formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupByInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="builder">The fluent builder associated with the handler.</param>
    /// <param name="isHandlerEnabled">The value that indicates whether the handler is enabled or disabled.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="builder"/> is <see langword="null"/> or doesn't implement <see cref="IFluentBuilderFormatter"/>.</exception>
    public GroupByInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupByInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="condition">The value to determine whether the handler should be enabled or disabled.</param>
    /// <param name="builder">The fluent builder associated with the handler.</param>
    /// <param name="isHandlerEnabled">The value that indicates whether the handler is enabled or disabled.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="builder"/> is <see langword="null"/> or doesn't implement <see cref="IFluentBuilderFormatter"/>.</exception>
    public GroupByInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        formatter = builder as IFluentBuilderFormatter
            ?? throw new ArgumentException($"The {nameof(builder)} must implement {nameof(IFluentBuilderFormatter)}.", nameof(builder));

        if (!(condition && formatter.IsClauseActionEnabled(ClauseAction.GroupBy)))
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.GroupBy);
    }

    /// <summary>
    /// Appends a string to the builder.
    /// </summary>
    /// <param name="value">The string to append.</param>
    public void AppendLiteral(string value)
        => formatter?.AppendLiteral(value);

    /// <summary>
    /// Appends a value to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to append.</param>
    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    /// <summary>
    /// Appends a value to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to append.</param>
    /// <param name="format">The format string for the value.</param>
    public void AppendFormatted<T>(T value, string? format)
        => formatter?.AppendFormatted(value, format);

    internal void Close()
        => formatter?.EndClauseAction();
}
#endif
