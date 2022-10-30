#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct SelectDistinctInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    internal SelectDistinctInterpolatedStringHandler(int literalLength, int formattedCount, IFluentSqlFormatter formatter)
    {
        this.formatter = formatter;
    }

    internal void AppendLiteral(string value)
        => formatter?.FormatLiteral(value, Clause.SelectDistinct);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatValue(value, Clause.SelectDistinct, format);
}
#endif
