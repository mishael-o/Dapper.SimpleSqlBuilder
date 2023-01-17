#if NET6_0_OR_GREATER

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

[InterpolatedStringHandler]
public ref struct UpdateInterpolatedStringHandler
{
    private readonly IFluentSqlFormatter formatter;

    public UpdateInterpolatedStringHandler(int literalLength, int formattedCount, IFluentBuilder builder)
    {
        formatter = builder as IFluentSqlFormatter ?? throw new ArgumentNullException(nameof(builder));
        formatter.StartClauseAction(ClauseAction.Update);
    }

    public void AppendLiteral(string value)
        => formatter.FormatLiteral(value);

    public void AppendFormatted<T>(T value)
        => AppendFormatted(value, null);

    public void AppendFormatted<T>(T value, string? format)
        => formatter.FormatParameter(value, format);

    internal void Close()
        => formatter.EndClauseAction(ClauseAction.Update);
}
#endif
