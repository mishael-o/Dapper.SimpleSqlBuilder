#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct SelectFromInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    internal SelectFromInterpolatedStringHandler(int literalLength, int formattedCount, IFluentSqlFormatter formatter)
    {
        this.formatter = formatter;
    }

    internal void AppendLiteral(string value)
        => formatter?.FormatLiteral(value, Clause.SelectFrom);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatValue(value, Clause.SelectFrom, format);
}
#endif
