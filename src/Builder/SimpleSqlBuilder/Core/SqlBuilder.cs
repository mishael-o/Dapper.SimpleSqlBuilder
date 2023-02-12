using System.Data;
using System.Text;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A class that defines the SQL builder type. The core <see cref="SqlBuilder"/> partial class.
/// </summary>
internal sealed partial class SqlBuilder : Builder
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
        AppendFormattable(formattable);
    }

    public override string Sql
        => stringBuilder.ToString();

    public override object Parameters
        => parameters;

    public override IEnumerable<string> ParameterNames
        => parameters.ParameterNames;

#if NET6_0_OR_GREATER
    public override Builder Append([InterpolatedStringHandlerArgument("")] ref AppendInterpolatedStringHandler handler)
        => this;

    public override Builder AppendIntact([InterpolatedStringHandlerArgument("")] ref AppendIntactInterpolatedStringHandler handler)
        => this;

    public override Builder AppendNewLine([InterpolatedStringHandlerArgument("")] ref AppendNewLineInterpolatedStringHandler handler)
        => this;
#else

    public override Builder Append(FormattableString formattable)
    {
        AppendSpace();
        AppendFormattable(formattable);
        return this;
    }

    public override Builder AppendIntact(FormattableString formattable)
    {
        AppendFormattable(formattable);
        return this;
    }

    public override Builder AppendNewLine(FormattableString formattable)
    {
        AppendNewLine();
        AppendFormattable(formattable);
        return this;
    }

#endif

    public override Builder AppendNewLine()
    {
        stringBuilder.AppendLine();
        return this;
    }

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

    private void AppendFormattable(FormattableString? formattable)
    {
        if (formattable is null)
        {
            return;
        }

        if (formattable.ArgumentCount == 0)
        {
            stringBuilder.Append(formattable.Format);
            return;
        }

        stringBuilder.AppendFormat(sqlFormatter, formattable.Format, formattable.GetArguments());
    }

    private void AppendSpace()
        => stringBuilder.Append(SpacePrefix);
}
