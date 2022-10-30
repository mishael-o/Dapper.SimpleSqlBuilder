namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Core <see cref="SimpleFluentBuilder"/> partial class.
/// </summary>
internal partial class SimpleFluentBuilder : IFormatProvider, ICustomFormatter, IFluentSqlFormatter
{
    private static readonly SimpleParameterInfoComparer Comparer = new();

    private int paramCount;
    private Dictionary<SimpleParameterInfo, string>? parameterDictionary;

    public object? GetFormat(Type? formatType)
    {
        return typeof(ICustomFormatter).IsAssignableFrom(formatType)
            ? this
            : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        => Format(arg, format);

    public void FormatLiteral(string value, Clause clause)
    {
        AppendClause(clause);
        stringBuilder.Append(value);
    }

    public void FormatValue<T>(T value, Clause clause, string? format = null)
    {
        AppendClause(clause);
        stringBuilder.Append(Format(value, format));
    }

    private string Format<T>(T value, string? format = null)
    {
        if (value is FormattableString formattableString)
        {
            return formattableString.ArgumentCount == 0
                ? formattableString.Format
                : string.Format(this, formattableString.Format, formattableString.GetArguments());
        }

        if (Constants.RawFormat.Equals(format, StringComparison.OrdinalIgnoreCase))
        {
            return value?.ToString() ?? string.Empty;
        }

        if ((value is null or not SimpleParameterInfo) && !reuseParameters)
        {
            return AddValueToParameters(value);
        }

        if (value is SimpleParameterInfo parameterInfo)
        {
            return AddParameterInfoToParameters(parameterInfo);
        }

        parameterInfo = new SimpleParameterInfo(value);
        return AddParameterInfoToParameters(parameterInfo);
    }

    private string AddValueToParameters<T>(T value)
    {
        var parameterName = GetNextParameterName();
        parameters.Add(parameterName, value, direction: System.Data.ParameterDirection.Input);
        return AppendParameterPrefix(parameterName);
    }

    private string AddParameterInfoToParameters(SimpleParameterInfo parameterInfo)
    {
        parameterDictionary ??= new(Comparer);

        if (parameterDictionary.TryGetValue(parameterInfo, out var dbPrefixedParameterName))
        {
            return dbPrefixedParameterName;
        }

        if (!parameterInfo.HasName)
        {
            parameterInfo.SetName(GetNextParameterName());
        }

        parameters.Add(parameterInfo.Name, parameterInfo.Value, parameterInfo.DbType, parameterInfo.Direction, parameterInfo.Size, parameterInfo.Precision, parameterInfo.Scale);

        dbPrefixedParameterName = AppendParameterPrefix(parameterInfo.Name!);

        if (parameterInfo.HasValue)
        {
            parameterDictionary[parameterInfo] = dbPrefixedParameterName;
        }

        return dbPrefixedParameterName;
    }

    private string GetNextParameterName()
        => $"{parameterNameTemplate}{paramCount++}";

    private string AppendParameterPrefix(string parameterName)
        => parameterPrefix + parameterName;

    private void AppendClause(Clause clause)
    {
        if (clause == Clause.None)
        {
            return;
        }

        switch (clause)
        {
            case Clause.Delete:
                AppendDelete();
                break;

            case Clause.Insert:
                AppendInsert();
                break;

            case Clause.InsertValue:
                AppendInsertValue();
                break;

            case Clause.Select:
                AppendSelect();
                break;

            case Clause.SelectDistinct:
                AppendSelectDistinct();
                break;

            case Clause.SelectFrom:
                AppendSelectFrom();
                break;

            case Clause.Update:
                AppendUpdate();
                break;

            case Clause.UpdateSet:
                AppendUpdateSet();
                break;

            case Clause.Where:
                AppendWhere();
                break;

            case Clause.WhereOr:
                AppendWhereOr();
                break;

            case Clause.InnerJoin:
                AppendInnerJoin();
                break;

            case Clause.LeftJoin:
                AppendLeftJoin();
                break;

            case Clause.RightJoin:
                AppendRightJoin();
                break;

            case Clause.GroupBy:
                AppendGroupBy();
                break;

            case Clause.Having:
                AppendHaving();
                break;

            case Clause.OrderBy:
                AppendOrderBy();
                break;
        }
    }

    private void AppendDelete()
    {
        if (clauses.Contains(Clause.Delete))
        {
            return;
        }

        clauses.Add(Clause.Delete);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Delete.Lower : ClauseConstants.Delete.Upper);
    }

    private void AppendInsert()
    {
        if (clauses.Contains(Clause.Insert))
        {
            return;
        }

        clauses.Add(Clause.Insert);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Insert.Lower : ClauseConstants.Insert.Upper);
    }

    private void AppendInsertValue()
    {
        if (clauses.Contains(Clause.InsertValue))
        {
            stringBuilder.Append(ClauseConstants.Insert.ValuesSeperator);
            return;
        }

        clauses.Add(Clause.InsertValue);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Insert.ValuesLower : ClauseConstants.Insert.Values);
    }

    private void AppendSelect()
    {
        if (clauses.Contains(Clause.Select))
        {
            return;
        }

        clauses.Add(Clause.Select);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.Lower : ClauseConstants.Select.Upper);
    }

    private void AppendSelectFrom()
    {
        if (clauses.Contains(Clause.SelectFrom))
        {
            return;
        }

        clauses.Add(Clause.SelectFrom);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.FromLower : ClauseConstants.Select.FromUpper);
    }

    private void AppendSelectDistinct()
    {
        if (clauses.Contains(Clause.SelectDistinct))
        {
            return;
        }

        clauses.Add(Clause.SelectDistinct);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.DistinctLower : ClauseConstants.Select.DistinctUpper);
    }

    private void AppendUpdate()
    {
        if (clauses.Contains(Clause.Update))
        {
            return;
        }

        clauses.Add(Clause.Update);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Update.Lower : ClauseConstants.Update.Upper);
    }

    private void AppendUpdateSet()
    {
        if (clauses.Contains(Clause.UpdateSet))
        {
            stringBuilder.Append(ClauseConstants.Update.SetSeperator);
            return;
        }

        clauses.Add(Clause.UpdateSet);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Update.SetLower : ClauseConstants.Update.SetUpper);
    }

    private void AppendWhere()
    {
        if (clauses.Contains(Clause.Where))
        {
            stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeperatorLower : ClauseConstants.Where.AndSeperator);
            return;
        }

        clauses.Add(Clause.Where);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.Lower : ClauseConstants.Where.Upper);
    }

    private void AppendWhereOr()
    {
        if (!clauses.Contains(Clause.WhereOr))
        {
            clauses.Add(Clause.WhereOr);
        }

        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.OrSeperatorLower : ClauseConstants.Where.OrSeperator);
    }

    private void AppendInnerJoin()
    {
        if (!clauses.Contains(Clause.InnerJoin))
        {
            clauses.Add(Clause.InnerJoin);
        }

        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerInnerJoin : ClauseConstants.Join.UpperInnerJoin);
    }

    private void AppendRightJoin()
    {
        if (!clauses.Contains(Clause.RightJoin))
        {
            clauses.Add(Clause.RightJoin);
        }

        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerRightJoin : ClauseConstants.Join.UpperRightJoin);
    }

    private void AppendLeftJoin()
    {
        if (!clauses.Contains(Clause.LeftJoin))
        {
            clauses.Add(Clause.LeftJoin);
        }

        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerLeftJoin : ClauseConstants.Join.UpperLeftJoin);
    }

    private void AppendGroupBy()
    {
        if (clauses.Contains(Clause.GroupBy))
        {
            stringBuilder.Append(ClauseConstants.GroupBy.Seperator);
            return;
        }

        clauses.Add(Clause.GroupBy);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.GroupBy.Lower : ClauseConstants.GroupBy.Upper);
    }

    private void AppendOrderBy()
    {
        if (clauses.Contains(Clause.OrderBy))
        {
            stringBuilder.Append(ClauseConstants.OrderBy.Seperator);
            return;
        }

        clauses.Add(Clause.OrderBy);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.OrderBy.Lower : ClauseConstants.OrderBy.Upper);
    }

    private void AppendHaving()
    {
        if (clauses.Contains(Clause.Having))
        {
            stringBuilder.Append(ClauseConstants.Having.Seperator);
            return;
        }

        clauses.Add(Clause.Having);
        stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Having.Lower : ClauseConstants.Having.Upper);
    }

    private string GetSql()
    {
        return clauses.Contains(Clause.InsertValue)
            ? stringBuilder.ToString() + ClauseConstants.Insert.Closer
            : stringBuilder.ToString();
    }
}
