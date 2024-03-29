﻿namespace Dapper.SimpleSqlBuilder;

internal interface IBuilderFormatter
{
    void AppendControl(ControlType controlType);

    void AppendFormatted<T>(T value, string? format = null);

    void AppendLiteral(string value);
}
