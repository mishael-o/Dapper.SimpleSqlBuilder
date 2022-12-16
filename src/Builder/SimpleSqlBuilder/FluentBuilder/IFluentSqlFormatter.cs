﻿namespace Dapper.SimpleSqlBuilder;

internal interface IFluentSqlFormatter
{
    void StartClauseAction(ClauseAction action);

    void EndClauseAction(ClauseAction action);

    void FormatLiteral(string value);

    void FormatParameter<T>(T value, string? format = null);

    bool IsClauseActionEnabled(ClauseAction action);
}
