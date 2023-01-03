#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct SelectInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter formatter;

    internal SelectInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder)
    {
        formatter = (IFluentSqlFormatter)builder;
        formatter.StartClauseAction(ClauseAction.Select);
    }

    internal void AppendLiteral(string value)
        => formatter.FormatLiteral(value);

    internal void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    internal void AppendFormatted<T>(T value, string? format)
        => formatter.FormatParameter(value, format);

    internal void Close()
        => formatter.EndClauseAction(ClauseAction.Select);
}
#endif
