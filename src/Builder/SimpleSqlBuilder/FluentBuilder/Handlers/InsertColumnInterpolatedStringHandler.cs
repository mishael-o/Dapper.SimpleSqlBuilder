#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// A handler used by the language compiler to append interpolated strings into <see cref="IFluentBuilder"/> instances.
/// </summary>
[InterpolatedStringHandler]
public ref struct InsertColumnInterpolatedStringHandler
{
    private readonly IFluentBuilderFormatter? formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="InsertColumnInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="builder">The fluent builder associated with the handler.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="builder"/> is <see langword="null"/> or doesn't implement <see cref="IFluentBuilderFormatter"/>.</exception>
    public InsertColumnInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder)
    {
        formatter = builder as IFluentBuilderFormatter
            ?? throw new ArgumentException($"The {nameof(builder)} must implement {nameof(IFluentBuilderFormatter)}.", nameof(builder));
        formatter.StartClauseAction(ClauseAction.InsertColumn);
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
