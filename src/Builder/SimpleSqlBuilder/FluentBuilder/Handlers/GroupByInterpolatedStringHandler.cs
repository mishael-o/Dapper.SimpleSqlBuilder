#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct GroupByInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    public GroupByInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    public GroupByInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));

        if (!(condition && formatter.IsClauseActionEnabled(ClauseAction.GroupBy)))
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.GroupBy);
    }

    public void AppendLiteral(string value)
        => formatter?.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter?.AppendFormatted(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.GroupBy);
}
#endif
