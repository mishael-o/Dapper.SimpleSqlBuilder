#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct GroupByInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    internal GroupByInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    internal GroupByInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        formatter = (IFluentSqlFormatter)builder;

        if (!(condition && formatter.IsClauseActionEnabled(ClauseAction.GroupBy)))
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.GroupBy);
    }

    internal void AppendLiteral(string value)
        => formatter?.FormatLiteral(value);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatParameter(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.GroupBy);
}
#endif
