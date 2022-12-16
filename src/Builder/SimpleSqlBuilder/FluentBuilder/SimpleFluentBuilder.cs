using System.Text;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Core <see cref="SimpleFluentBuilder"/> partial class.
/// </summary>
internal partial class SimpleFluentBuilder
{
    private readonly bool reuseParameters;
    private readonly string parameterPrefix;
    private readonly bool useLowerCaseClauses;
    private readonly string parameterNameTemplate;
    private readonly StringBuilder stringBuilder = new();
    private readonly DynamicParameters parameters = new();
    private readonly List<ClauseAction> clauseActions = new();

    private int paramCount;
    private bool hasOpenParentheses;
    private ClauseAction pendingWhereFilter;
    private Dictionary<SimpleParameterInfo, string>? parameterDictionary;

    public SimpleFluentBuilder(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        this.parameterNameTemplate = parameterNameTemplate;
        this.parameterPrefix = parameterPrefix;
        this.reuseParameters = reuseParameters;
        this.useLowerCaseClauses = useLowerCaseClauses;
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
        parameterDictionary ??= new(SimpleParameterInfoComparer.StaticInstance);

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

    private void AppendClause(ClauseAction clauseAction)
    {
        if (clauseAction == ClauseAction.None)
        {
            return;
        }

        switch (clauseAction)
        {
            case ClauseAction.Delete:
                AppendDelete();
                break;

            case ClauseAction.Insert:
                AppendInsert();
                break;

            case ClauseAction.Insert_Value:
                AppendInsertValue();
                break;

            case ClauseAction.Select:
            case ClauseAction.Select_Distinct:
                AppendSelect(clauseAction);
                break;

            case ClauseAction.Select_From:
                AppendSelectFrom();
                break;

            case ClauseAction.Update:
                AppendUpdate();
                break;

            case ClauseAction.Update_Set:
                AppendUpdateSet();
                break;

            case ClauseAction.Where:
                AppendWhere();
                break;

            case ClauseAction.Where_Filter:
                AppendWhere(true);
                break;

            case ClauseAction.Where_Or:
                AppendWhereOr();
                break;

            case ClauseAction.Where_With_Filter:
            case ClauseAction.Where_With_Or_Filter:
                AppendWhereWithFilter(clauseAction);
                break;

            case ClauseAction.Where_Or_Filter:
                AppendWhereOr(true);
                break;

            case ClauseAction.InnerJoin:
            case ClauseAction.LeftJoin:
            case ClauseAction.RightJoin:
                AppendJoin(clauseAction);
                break;

            case ClauseAction.GroupBy:
                AppendGroupBy();
                break;

            case ClauseAction.Having:
                AppendHaving();
                break;

            case ClauseAction.OrderBy:
                AppendOrderBy();
                break;
        }

        if (pendingWhereFilter is not ClauseAction.None)
        {
            pendingWhereFilter = ClauseAction.None;
        }
    }

    private void AppendDelete()
    {
        if (clauseActions.Contains(ClauseAction.Delete))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Delete);
        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Delete.Lower : ClauseConstants.Delete.Upper)
            .Append(ClauseConstants.Space);
    }

    private void AppendInsert()
    {
        if (clauseActions.Contains(ClauseAction.Insert))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Insert);
        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Insert.Lower : ClauseConstants.Insert.Upper)
            .Append(ClauseConstants.Space);
    }

    private void AppendInsertValue()
    {
        hasOpenParentheses = true;

        if (clauseActions.Contains(ClauseAction.Insert_Value))
        {
            stringBuilder.Length--;

            stringBuilder
                .Append(ClauseConstants.Insert.ValuesSeperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.Insert_Value);
        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Insert.ValuesLower : ClauseConstants.Insert.Values)
            .Append(ClauseConstants.Space)
            .Append(ClauseConstants.OpenParentheses);
    }

    private void AppendSelect(ClauseAction clauseAction)
    {
        if (clauseAction is not ClauseAction.Select and not ClauseAction.Select_Distinct)
        {
            throw new ArgumentException($"The clause argument ({clauseAction}) is invalid for this method.", nameof(clauseAction));
        }

        if (clauseActions.Contains(clauseAction))
        {
            stringBuilder
                .Append(ClauseConstants.Select.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        switch (clauseAction)
        {
            case ClauseAction.Select:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.Lower : ClauseConstants.Select.Upper);
                break;

            case ClauseAction.Select_Distinct:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.DistinctLower : ClauseConstants.Select.DistinctUpper);
                break;
        }

        clauseActions.Add(clauseAction);
        stringBuilder.Append(ClauseConstants.Space);
    }

    private void AppendSelectFrom()
    {
        if (clauseActions.Contains(ClauseAction.Select_From))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Select_From);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Select.FromLower : ClauseConstants.Select.FromUpper)
            .Append(ClauseConstants.Space);
    }

    private void AppendUpdate()
    {
        if (clauseActions.Contains(ClauseAction.Update))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Update);
        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Update.Lower : ClauseConstants.Update.Upper)
            .Append(ClauseConstants.Space);
    }

    private void AppendUpdateSet()
    {
        if (clauseActions.Contains(ClauseAction.Update_Set))
        {
            stringBuilder
                .Append(ClauseConstants.Update.SetSeperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.Update_Set);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Update.SetLower : ClauseConstants.Update.SetUpper);
    }

    private void AppendWhere(bool isFilter = false)
    {
        if (clauseActions.Contains(ClauseAction.Where))
        {
            stringBuilder
                .Append(ClauseConstants.Space)
                .Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeperatorLower : ClauseConstants.Where.AndSeperator);
        }
        else
        {
            clauseActions.Add(ClauseAction.Where);
            stringBuilder
                .AppendLine()
                .Append(useLowerCaseClauses ? ClauseConstants.Where.Lower : ClauseConstants.Where.Upper);
        }

        stringBuilder.Append(ClauseConstants.Space);

        if (isFilter)
        {
            stringBuilder.Append(ClauseConstants.OpenParentheses);
            hasOpenParentheses = true;
        }
    }

    private void AppendWhereOr(bool isFilter = false)
    {
        if (!clauseActions.Contains(ClauseAction.Where))
        {
            AppendWhere(isFilter);
            return;
        }

        if (!clauseActions.Contains(ClauseAction.Where_Or))
        {
            clauseActions.Add(ClauseAction.Where_Or);
        }

        stringBuilder
            .Append(ClauseConstants.Space)
            .Append(useLowerCaseClauses ? ClauseConstants.Where.OrSeperatorLower : ClauseConstants.Where.OrSeperator)
            .Append(ClauseConstants.Space);

        if (isFilter)
        {
            stringBuilder.Append(ClauseConstants.OpenParentheses);
            hasOpenParentheses = true;
        }
    }

    private void AppendWhereWithFilter(ClauseAction clauseAction)
    {
        if (clauseAction is not ClauseAction.Where_With_Filter and not ClauseAction.Where_With_Or_Filter)
        {
            throw new ArgumentException($"The clause argument ({clauseAction}) is invalid for this method.", nameof(clauseAction));
        }

        if (clauseAction is ClauseAction.Where_With_Filter && pendingWhereFilter is not ClauseAction.None)
        {
            switch (pendingWhereFilter)
            {
                case ClauseAction.Where_Filter:
                    AppendWhere(true);
                    break;

                case ClauseAction.Where_Or_Filter:
                    AppendWhereOr(true);
                    break;

                default:
                    throw new InvalidOperationException($"The pending where filter ({pendingWhereFilter}) is invalid.");
            }

            return;
        }

        if (!clauseActions.Contains(clauseAction))
        {
            clauseActions.Add(clauseAction);
        }

        stringBuilder.Length--;
        stringBuilder.Append(ClauseConstants.Space);

        switch (clauseAction)
        {
            case ClauseAction.Where_With_Filter:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeperatorLower : ClauseConstants.Where.AndSeperator);
                break;

            case ClauseAction.Where_With_Or_Filter:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.OrSeperatorLower : ClauseConstants.Where.OrSeperator);
                break;
        }

        stringBuilder.Append(ClauseConstants.Space);
        hasOpenParentheses = true;
    }

    private void AppendJoin(ClauseAction clauseAction)
    {
        if (clauseAction is not ClauseAction.InnerJoin and not ClauseAction.LeftJoin and not ClauseAction.RightJoin)
        {
            throw new ArgumentException($"The clause argument ({clauseAction}) is invalid for this method.", nameof(clauseAction));
        }

        if (!clauseActions.Contains(clauseAction))
        {
            clauseActions.Add(clauseAction);
        }

        stringBuilder.AppendLine();

        switch (clauseAction)
        {
            case ClauseAction.InnerJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerInnerJoin : ClauseConstants.Join.UpperInnerJoin);
                break;

            case ClauseAction.LeftJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerLeftJoin : ClauseConstants.Join.LowerLeftJoin);
                break;

            case ClauseAction.RightJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerRightJoin : ClauseConstants.Join.LowerRightJoin);
                break;
        }

        stringBuilder.Append(ClauseConstants.Space);
    }

    private void AppendGroupBy()
    {
        if (clauseActions.Contains(ClauseAction.GroupBy))
        {
            stringBuilder
                .Append(ClauseConstants.GroupBy.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.GroupBy);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.GroupBy.Lower : ClauseConstants.GroupBy.Upper)
            .Append(ClauseConstants.Space);
    }

    private void AppendOrderBy()
    {
        if (clauseActions.Contains(ClauseAction.OrderBy))
        {
            stringBuilder
                .Append(ClauseConstants.OrderBy.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.OrderBy);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.OrderBy.Lower : ClauseConstants.OrderBy.Upper)
            .Append(ClauseConstants.Space);
    }

    private void AppendHaving()
    {
        if (clauseActions.Contains(ClauseAction.Having))
        {
            stringBuilder
                .Append(ClauseConstants.Having.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.Having);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Having.Lower : ClauseConstants.Having.Upper)
            .Append(ClauseConstants.Space);
    }

    private void CloseOpenParentheses()
    {
        if (!hasOpenParentheses)
        {
            return;
        }

        stringBuilder.Append(ClauseConstants.CloseParentheses);
        hasOpenParentheses = false;
    }
}
