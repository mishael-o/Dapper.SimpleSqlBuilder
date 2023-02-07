#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct OrderByInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    public OrderByInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    public OrderByInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));

        if (!(condition && formatter.IsClauseActionEnabled(ClauseAction.OrderBy)))
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.OrderBy);
    }

    public void AppendLiteral(string value)
        => formatter?.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter?.AppendFormatted(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.OrderBy);
}
#endif
