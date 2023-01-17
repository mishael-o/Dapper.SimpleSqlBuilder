﻿#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct RightJoinInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter? formatter;

    public RightJoinInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder, out bool isHandlerEnabled)
        : this(literalLength, formattedCount, true, builder, out isHandlerEnabled)
    {
    }

    public RightJoinInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, IFluentBuilder builder, out bool isHandlerEnabled)
    {
        if (!condition)
        {
            formatter = default;
            isHandlerEnabled = false;
            return;
        }

        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));
        isHandlerEnabled = true;
        formatter.StartClauseAction(ClauseAction.RightJoin);
    }

    public void AppendLiteral(string value)
        => formatter?.FormatLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter?.FormatParameter(value, format);

    internal void Close()
        => formatter?.EndClauseAction(ClauseAction.RightJoin);
}

#endif
