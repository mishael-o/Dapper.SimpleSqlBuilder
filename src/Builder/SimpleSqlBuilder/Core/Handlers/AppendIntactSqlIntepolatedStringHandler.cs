#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct AppendIntactSqlIntepolatedStringHandler
{
    private readonly IBuilderFormatter formatter;

    public AppendIntactSqlIntepolatedStringHandler(int literalLength, int formattedCount, ISqlBuilder builder)
    {
        formatter = builder as IBuilderFormatter
            ?? throw new ArgumentException($"The {nameof(builder)} must implement {nameof(IBuilderFormatter)}.", nameof(builder));
    }

    public void AppendLiteral(string value)
        => formatter.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter.AppendFormatted(value, format);
}
#endif
