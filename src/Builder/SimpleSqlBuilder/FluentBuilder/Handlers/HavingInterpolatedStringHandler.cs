#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct HavingInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    public HavingInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    public HavingInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));

        if (!(condition && formatter.IsClauseActionEnabled(ClauseAction.Having)))
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.Having);
    }

    public void AppendLiteral(string value)
        => formatter?.FormatLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatParameter(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.Having);
}
#endif
