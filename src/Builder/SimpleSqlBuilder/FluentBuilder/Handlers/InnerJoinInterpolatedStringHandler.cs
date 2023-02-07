#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct InnerJoinInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    public InnerJoinInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    public InnerJoinInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        if (!condition)
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));
        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.InnerJoin);
    }

    public void AppendLiteral(string value)
        => formatter?.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter?.AppendFormatted(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.InnerJoin);
}

#endif
