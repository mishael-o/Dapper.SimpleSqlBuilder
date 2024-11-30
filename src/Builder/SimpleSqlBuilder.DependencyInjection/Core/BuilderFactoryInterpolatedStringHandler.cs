#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace Dapper.SimpleSqlBuilder.DependencyInjection;

/// <summary>
/// A handler used by the language compiler to create the <see cref="Builder"/> and append interpolated strings into <see cref="Builder"/> instances.
/// </summary>
[InterpolatedStringHandler]
public ref struct BuilderFactoryInterpolatedStringHandler
{
    private readonly IBuilderFormatter? formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuilderFactoryInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="builderFactory">The <see cref="ISimpleBuilder"/> object for creating builder instances.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builderFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when created builder does not implement <see cref="IBuilderFormatter"/>.</exception>
    public BuilderFactoryInterpolatedStringHandler(int literalLength, int formattedCount, ISimpleBuilder builderFactory)
    {
        ArgumentNullException.ThrowIfNull(builderFactory);

        formatter = builderFactory.Create() as IBuilderFormatter
            ?? throw new InvalidOperationException($"{nameof(ISimpleBuilder)}.{nameof(ISimpleBuilder.Create)} does not return a builder that implements {nameof(IBuilderFormatter)}.");
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
        => formatter?.AppendFormatted(value);

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
            ? throw new InvalidOperationException($"The {nameof(formatter)} is null. Ensure {nameof(BuilderFactoryInterpolatedStringHandler)} is properly initialized.")
            : (Builder)formatter;
    }
}
#endif
