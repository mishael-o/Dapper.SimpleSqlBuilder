#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct InsertInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    internal InsertInterpolatedStringHandler(int literalLength, int formattedCount, IFluentSqlFormatter formatter)
    {
        this.formatter = formatter;
    }

    internal void AppendLiteral(string value)
        => formatter?.FormatLiteral(value, Clause.Insert);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatValue(value, Clause.Insert, format);
}
#endif
