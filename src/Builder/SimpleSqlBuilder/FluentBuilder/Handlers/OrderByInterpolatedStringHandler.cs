#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct OrderByInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    internal OrderByInterpolatedStringHandler(int literalLength, int formattedCount, IFluentSqlFormatter formatter)
    {
        this.formatter = formatter;
    }

    internal OrderByInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentSqlFormatter formatter, out bool isHandlerEnabled)
    {
        if (!condition)
        {
            this.formatter = default;
            isHandlerEnabled = false;
            return;
        }

        this.formatter = formatter;
        isHandlerEnabled = true;
    }

    internal void AppendLiteral(string value)
        => formatter?.FormatLiteral(value, Clause.OrderBy);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatValue(value, Clause.OrderBy, format);
}
#endif
