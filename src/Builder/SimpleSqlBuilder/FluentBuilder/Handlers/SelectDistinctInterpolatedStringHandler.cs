﻿#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct SelectDistinctInterpolatedStringHandler
{
    private readonly IFluentBuilderFormatter formatter;

    public SelectDistinctInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder)
    {
        formatter = builder as IFluentBuilderFormatter
            ?? throw new ArgumentException($"The {nameof(builder)} must implement {nameof(IFluentBuilderFormatter)}.", nameof(builder));
        formatter.StartClauseAction(ClauseAction.SelectDistinct);
    }

    public void AppendLiteral(string value)
        => formatter.AppendLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter.AppendFormatted(value, format);

    internal void Close()
        => formatter.EndClauseAction(ClauseAction.SelectDistinct);
}
#endif
