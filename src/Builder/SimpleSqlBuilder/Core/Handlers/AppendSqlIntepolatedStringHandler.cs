#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct AppendSqlIntepolatedStringHandler
{
    private readonly IBuilderFormatter formatter;

    public AppendSqlIntepolatedStringHandler(int literalLength, int formattedCount, ISqlBuilder builder)
    {
        formatter = builder as IBuilderFormatter
            ?? throw new ArgumentException($"The {nameof(builder)} must implement {nameof(IBuilderFormatter)}.", nameof(builder));
        formatter.AppendControl(ControlType.Space);
    }

    public void AppendLiteral(string value)
        => formatter.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter.AppendFormatted(value, format);
}
#endif
