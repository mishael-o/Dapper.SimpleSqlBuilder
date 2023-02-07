namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// Implementation of <see cref="IFluentSqlFormatter"/> for <see cref="FluentSqlBuilder"/> partial class.
/// </summary>
internal partial class FluentSqlBuilder : IFluentSqlFormatter
{
    public void AppendLiteral(string value)
        => stringBuilder.Append(value);

    public void AppendFormatted<T>(T value, string? format = null)
        => stringBuilder.Append(sqlFormatter.Format(value, format));

    public void EndClauseAction(ClauseAction clauseAction)
        => CloseOpenParentheses();

    public bool IsClauseActionEnabled(ClauseAction clauseAction)
    {
        return !clauseActions.Exists(c => c is ClauseAction.Delete or ClauseAction.Update)
            || clauseAction is not ClauseAction.GroupBy and not ClauseAction.Having and not ClauseAction.OrderBy;
    }

    public void StartClauseAction(ClauseAction clauseAction)
        => AppendClause(clauseAction);
}
