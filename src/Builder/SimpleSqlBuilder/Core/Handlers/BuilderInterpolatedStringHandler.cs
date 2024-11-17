#if NET6_0_OR_GREATER
namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A handler used by the language compiler to create the <see cref="Builder"/> and append interpolated strings into <see cref="Builder"/> instances.
/// </summary>
[InterpolatedStringHandler]
public ref struct BuilderInterpolatedStringHandler
{
    private readonly IBuilderFormatter? formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuilderInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <exception cref="InvalidOperationException">Thrown when created builder does not implement <see cref="IBuilderFormatter"/>.</exception>
    public BuilderInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        formatter = SimpleBuilder.Create() as IBuilderFormatter
            ?? throw new InvalidOperationException($"{nameof(SimpleBuilder)}.{nameof(SimpleBuilder.Create)} does not return a builder that implements {nameof(IBuilderFormatter)}.");
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

    internal Builder GetBuilder()
    {
        return formatter is null
            ? throw new InvalidOperationException($"The {nameof(formatter)} is null. Ensure {nameof(BuilderInterpolatedStringHandler)} is properly initialized.")
            : (Builder)formatter;
    }
}
#endif
