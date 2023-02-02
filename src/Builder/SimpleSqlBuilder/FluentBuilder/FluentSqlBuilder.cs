using System.Text;

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// Core <see cref="FluentSqlBuilder"/> partial class.
/// </summary>
internal partial class FluentSqlBuilder
{
    private readonly bool useLowerCaseClauses;
    private readonly StringBuilder stringBuilder = new();
    private readonly DynamicParameters parameters = new();
    private readonly List<ClauseAction> clauseActions = new();
    private readonly SqlFormatter sqlFormatter;

    private bool hasOpenParentheses;
    private ClauseAction pendingWhereFilter;

    public FluentSqlBuilder(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        this.useLowerCaseClauses = useLowerCaseClauses;
        sqlFormatter = new SqlFormatter(parameters, parameterNameTemplate, parameterPrefix, reuseParameters);
    }

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

            case ClauseAction.InsertColumn:
                AppendInsertColumn();
                break;

            case ClauseAction.InsertValue:
                AppendInsertValue();
                break;

            case ClauseAction.Select:
            case ClauseAction.SelectDistinct:
                AppendSelect(clauseAction);
                break;

            case ClauseAction.SelectFrom:
                AppendSelectFrom();
                break;

            case ClauseAction.Update:
                AppendUpdate();
                break;

            case ClauseAction.UpdateSet:
                AppendUpdateSet();
                break;

            case ClauseAction.Where:
                AppendWhere();
                break;

            case ClauseAction.WhereFilter:
                AppendWhere(true);
                break;

            case ClauseAction.WhereOr:
                AppendWhereOr();
                break;

            case ClauseAction.WhereWithFilter:
            case ClauseAction.WhereWithOrFilter:
                AppendWhereWithFilter(clauseAction);
                break;

            case ClauseAction.WhereOrFilter:
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

    private void AppendInsertColumn()
    {
        hasOpenParentheses = true;

        if (clauseActions.Contains(ClauseAction.InsertColumn))
        {
            stringBuilder.Length--;

            stringBuilder
                .Append(ClauseConstants.Insert.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.InsertColumn);
        stringBuilder
            .Append(ClauseConstants.Space)
            .Append(ClauseConstants.OpenParentheses);
    }

    private void AppendInsertValue()
    {
        hasOpenParentheses = true;

        if (clauseActions.Contains(ClauseAction.InsertValue))
        {
            stringBuilder.Length--;

            stringBuilder
                .Append(ClauseConstants.Insert.Seperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.InsertValue);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Insert.ValuesLower : ClauseConstants.Insert.Values)
            .Append(ClauseConstants.Space)
            .Append(ClauseConstants.OpenParentheses);
    }

    private void AppendSelect(ClauseAction clauseAction)
    {
        if (clauseAction is not ClauseAction.Select and not ClauseAction.SelectDistinct)
        {
            throw new ArgumentException($"The clause action ({clauseAction}) is invalid for this method.", nameof(clauseAction));
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

            case ClauseAction.SelectDistinct:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Select.DistinctLower : ClauseConstants.Select.DistinctUpper);
                break;
        }

        clauseActions.Add(clauseAction);
        stringBuilder.Append(ClauseConstants.Space);
    }

    private void AppendSelectFrom()
    {
        if (clauseActions.Contains(ClauseAction.SelectFrom))
        {
            return;
        }

        clauseActions.Add(ClauseAction.SelectFrom);
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
        if (clauseActions.Contains(ClauseAction.UpdateSet))
        {
            stringBuilder
                .Append(ClauseConstants.Update.SetSeperator)
                .Append(ClauseConstants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.UpdateSet);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Update.SetLower : ClauseConstants.Update.SetUpper)
            .Append(ClauseConstants.Space);
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

        if (!clauseActions.Contains(ClauseAction.WhereOr))
        {
            clauseActions.Add(ClauseAction.WhereOr);
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
        if (clauseAction is not ClauseAction.WhereWithFilter and not ClauseAction.WhereWithOrFilter)
        {
            throw new ArgumentException($"The clause action ({clauseAction}) is invalid for this method.", nameof(clauseAction));
        }

        if (pendingWhereFilter is not ClauseAction.None)
        {
            switch (pendingWhereFilter)
            {
                case ClauseAction.WhereFilter:
                    AppendWhere(true);
                    return;

                case ClauseAction.WhereOrFilter:
                    AppendWhereOr(true);
                    return;

                default:
                    throw new InvalidOperationException($"The pending where filter ({pendingWhereFilter}) is invalid.");
            }
        }

        if (!clauseActions.Contains(clauseAction))
        {
            clauseActions.Add(clauseAction);
        }

        stringBuilder.Length--;
        stringBuilder.Append(ClauseConstants.Space);

        switch (clauseAction)
        {
            case ClauseAction.WhereWithFilter:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeperatorLower : ClauseConstants.Where.AndSeperator);
                break;

            case ClauseAction.WhereWithOrFilter:
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
            throw new ArgumentException($"The clause action ({clauseAction}) is invalid for this method.", nameof(clauseAction));
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
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerLeftJoin : ClauseConstants.Join.UpperLeftJoin);
                break;

            case ClauseAction.RightJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LowerRightJoin : ClauseConstants.Join.UpperRightJoin);
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
                .Append(ClauseConstants.Space)
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
