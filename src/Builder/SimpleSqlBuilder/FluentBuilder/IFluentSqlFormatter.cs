namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal interface IFluentSqlFormatter : IFluentBuilder
{
    void AppendLiteral(string value);

    void AppendFormatted<T>(T value, string? format = null);

    void EndClauseAction(ClauseAction action);

    bool IsClauseActionEnabled(ClauseAction action);

    void StartClauseAction(ClauseAction action);
}
