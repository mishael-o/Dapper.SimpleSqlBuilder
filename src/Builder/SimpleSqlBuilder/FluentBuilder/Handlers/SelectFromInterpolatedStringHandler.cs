﻿#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder;

[InterpolatedStringHandler]
public ref struct SelectFromInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter formatter;

    internal SelectFromInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder)
    {
        formatter = (IFluentSqlFormatter)builder;
        formatter.StartClauseAction(ClauseAction.Select_From);
    }

    internal void AppendLiteral(string value)
        => formatter.FormatLiteral(value);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter.FormatParameter(value, format);

    internal void Close()
        => formatter.EndClauseAction(ClauseAction.Select_From);
}
#endif
