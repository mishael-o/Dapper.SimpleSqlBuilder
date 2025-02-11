﻿using System.Data;
using System.Text;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// A class that defines the SQL builder type. The core <see cref="SqlBuilder"/> partial class.
/// </summary>
internal sealed partial class SqlBuilder : Builder
{
    private readonly StringBuilder stringBuilder;
    private readonly SqlFormatter sqlFormatter;

    public SqlBuilder(ParameterOptions parameterOptions, FormattableString? formattable = null)
    {
        stringBuilder = new();
        sqlFormatter = new(parameterOptions);
        AppendFormattable(formattable);
    }

    public override string Sql
        => stringBuilder.ToString();

    public override object Parameters
        => sqlFormatter.Parameters;

    public override IEnumerable<string> ParameterNames
        => sqlFormatter.Parameters.ParameterNames;

#if NET6_0_OR_GREATER
    public override Builder Append([InterpolatedStringHandlerArgument("")] ref AppendInterpolatedStringHandler handler)
        => this;

    public override Builder Append(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendInterpolatedStringHandler handler)
        => this;

    public override Builder AppendIntact([InterpolatedStringHandlerArgument("")] ref AppendIntactInterpolatedStringHandler handler)
        => this;

    public override Builder AppendIntact(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendIntactInterpolatedStringHandler handler)
        => this;

    public override Builder AppendNewLine([InterpolatedStringHandlerArgument("")] ref AppendNewLineInterpolatedStringHandler handler)
        => this;

    public override Builder AppendNewLine(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref AppendNewLineInterpolatedStringHandler handler)
        => this;
#else

    public override Builder Append(FormattableString formattable)
    {
        AppendSpace();
        AppendFormattable(formattable);
        return this;
    }

    public override Builder Append(bool condition, FormattableString formattable)
    {
        return condition
            ? Append(formattable)
            : this;
    }

    public override Builder AppendIntact(FormattableString formattable)
    {
        AppendFormattable(formattable);
        return this;
    }

    public override Builder AppendIntact(bool condition, FormattableString formattable)
    {
        return condition
            ? AppendIntact(formattable)
            : this;
    }

    public override Builder AppendNewLine(FormattableString formattable)
    {
        AppendNewLine();
        AppendFormattable(formattable);
        return this;
    }

    public override Builder AppendNewLine(bool condition, FormattableString formattable)
    {
        return condition
            ? AppendNewLine(formattable)
            : this;
    }

#endif

    public override Builder AppendNewLine()
    {
        stringBuilder.AppendLine();
        return this;
    }

    public override Builder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        sqlFormatter.Parameters.Add(name, value, dbType, direction, size, precision, scale);
        return this;
    }

    public override Builder AddDynamicParameters(object? parameter)
    {
        sqlFormatter.Parameters.AddDynamicParams(parameter);
        return this;
    }

    public override T GetValue<T>(string parameterName)
        => sqlFormatter.Parameters.Get<T>(parameterName);

    public override void Reset()
    {
        sqlFormatter.Reset();
        stringBuilder.Clear();
    }

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
        => stringBuilder.Append(Constants.Space);
}
