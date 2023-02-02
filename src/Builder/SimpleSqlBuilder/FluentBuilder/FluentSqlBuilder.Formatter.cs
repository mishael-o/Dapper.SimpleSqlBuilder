namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// Implementation of <see cref="IFluentSqlFormatter"/> for <see cref="FluentSqlBuilder"/> partial class.
/// </summary>
internal partial class FluentSqlBuilder : IFluentSqlFormatter
{
    public void StartClauseAction(ClauseAction clauseAction)
        => AppendClause(clauseAction);

    public void EndClauseAction(ClauseAction clauseAction)
        => CloseOpenParentheses();

    public void FormatLiteral(string value)
        => stringBuilder.Append(value);

    public void FormatParameter<T>(T value, string? format = null)
        => stringBuilder.Append(sqlFormatter.Format(value, format));

    public bool IsClauseActionEnabled(ClauseAction clauseAction)
    {
        return !clauseActions.Exists(c => c is ClauseAction.Delete or ClauseAction.Update)
            || clauseAction is not ClauseAction.GroupBy and not ClauseAction.Having and not ClauseAction.OrderBy;
    }
}
