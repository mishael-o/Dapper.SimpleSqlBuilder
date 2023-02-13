namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// Implements the <see cref="IFluentBuilderFormatter"/> interface for the <see cref="FluentSqlBuilder"/> type.
/// </summary>
internal sealed partial class FluentSqlBuilder : IFluentBuilderFormatter
{
    public void AppendFormatted<T>(T value, string? format = null)
        => stringBuilder.Append(sqlFormatter.Format(value, format));

    public void AppendLiteral(string value)
        => stringBuilder.Append(value);

    public void EndClauseAction()
        => CloseOpenParentheses();

    public bool IsClauseActionEnabled(ClauseAction clauseAction)
        => CanAppendClause(clauseAction);

    public void StartClauseAction(ClauseAction clauseAction)
        => AppendClause(clauseAction);
}
