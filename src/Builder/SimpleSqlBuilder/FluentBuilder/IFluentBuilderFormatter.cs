namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal interface IFluentBuilderFormatter : IFluentBuilder
{
    void AppendFormatted<T>(T value, string? format = null);

    void AppendLiteral(string value);

    void EndClauseAction();

    bool IsClauseActionEnabled(ClauseAction action);

    void StartClauseAction(ClauseAction action);
}
