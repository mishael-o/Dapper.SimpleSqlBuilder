using System.Data;
using System.Text;

namespace Dapper.SimpleSqlBuilder;

internal sealed class SqlBuilder : Builder
{
    private const char SpacePrefix = ' ';

    private readonly StringBuilder stringBuilder;
    private readonly SqlFormatter sqlFormatter;
    private readonly DynamicParameters parameters;

    public SqlBuilder(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, FormattableString? formattable = null)
    {
        stringBuilder = new();
        parameters = new();
        sqlFormatter = new(parameters, parameterNameTemplate, parameterPrefix, reuseParameters);

        if (formattable is not null)
        {
            AppendSql(formattable);
        }
    }

    public override string Sql
        => stringBuilder.ToString();

    public override object Parameters
        => parameters;

    public override IEnumerable<string> ParameterNames
        => parameters.ParameterNames;

    public override Builder Append(FormattableString formattable)
        => AppendSql(formattable, addSpacePrefix: true);

    public override Builder AppendIntact(FormattableString formattable)
        => AppendSql(formattable);

    public override Builder AppendNewLine(FormattableString? formattable = null)
        => AppendSql(formattable, startNewLine: true);

    public override Builder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        parameters.Add(name, value, dbType, direction, size, precision, scale);
        return this;
    }

    public override Builder AddDynamicParameters(object? parameter)
    {
        parameters.AddDynamicParams(parameter);
        return this;
    }

    public override T GetValue<T>(string parameterName)
        => parameters.Get<T>(parameterName);

    private Builder AppendSql(FormattableString? formattable, bool startNewLine = false, bool addSpacePrefix = false)
    {
        if (startNewLine)
        {
            stringBuilder.AppendLine();
        }

        if (formattable is null)
        {
            return this;
        }

        if (addSpacePrefix)
        {
            stringBuilder.Append(SpacePrefix);
        }

        if (formattable.ArgumentCount == 0)
        {
            stringBuilder.Append(formattable.Format);
        }
        else
        {
            stringBuilder.AppendFormat(sqlFormatter, formattable.Format, formattable.GetArguments());
        }

        return this;
    }
}
