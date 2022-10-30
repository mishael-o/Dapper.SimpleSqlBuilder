namespace Dapper.SimpleSqlBuilder;

internal interface IFluentSqlFormatter
{
    void FormatLiteral(string value, Clause clause);

    void FormatValue<T>(T value, Clause clause, string? format = null);
}
