using System.Text;

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// A class that defines the fluent SQL builder type. The core <see cref="FluentSqlBuilder"/> partial class.
/// </summary>
internal sealed partial class FluentSqlBuilder
{
    private readonly bool useLowerCaseClauses;
    private readonly StringBuilder stringBuilder;
    private readonly DynamicParameters parameters;
    private readonly List<ClauseAction> clauseActions;
    private readonly SqlFormatter sqlFormatter;

    private bool hasOpenParentheses;
    private ClauseAction pendingWhereFilter;

    public FluentSqlBuilder(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        this.useLowerCaseClauses = useLowerCaseClauses;

        stringBuilder = new();
        parameters = new();
        clauseActions = new();
        sqlFormatter = new(parameters, parameterNameTemplate, parameterPrefix, reuseParameters);
    }

    private void AppendClause(ClauseAction clauseAction)
    {
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
            .Append(Constants.Space);
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
            .Append(Constants.Space);
    }

    private void AppendInsertColumn()
    {
        hasOpenParentheses = true;

        if (clauseActions.Contains(ClauseAction.InsertColumn))
        {
            stringBuilder.Length--;

            stringBuilder
                .Append(ClauseConstants.Insert.Separator)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.InsertColumn);
        stringBuilder
            .Append(Constants.Space)
            .Append(ClauseConstants.OpenParentheses);
    }

    private void AppendInsertValue()
    {
        hasOpenParentheses = true;

        if (clauseActions.Contains(ClauseAction.InsertValue))
        {
            stringBuilder.Length--;

            stringBuilder
                .Append(ClauseConstants.Insert.Separator)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.InsertValue);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Insert.ValuesLower : ClauseConstants.Insert.ValuesUpper)
            .Append(Constants.Space)
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
                .Append(ClauseConstants.Select.Separator)
                .Append(Constants.Space);

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
        stringBuilder.Append(Constants.Space);
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
            .Append(Constants.Space);
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
            .Append(Constants.Space);
    }

    private void AppendUpdateSet()
    {
        if (clauseActions.Contains(ClauseAction.UpdateSet))
        {
            stringBuilder
                .Append(ClauseConstants.Update.SetSeparator)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.UpdateSet);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Update.SetLower : ClauseConstants.Update.SetUpper)
            .Append(Constants.Space);
    }

    private void AppendWhere(bool isFilter = false)
    {
        if (clauseActions.Contains(ClauseAction.Where))
        {
            stringBuilder
                .Append(Constants.Space)
                .Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeparatorLower : ClauseConstants.Where.AndSeparatorUpper);
        }
        else
        {
            clauseActions.Add(ClauseAction.Where);
            stringBuilder
                .AppendLine()
                .Append(useLowerCaseClauses ? ClauseConstants.Where.Lower : ClauseConstants.Where.Upper);
        }

        stringBuilder.Append(Constants.Space);

        if (!isFilter)
        {
            return;
        }

        stringBuilder.Append(ClauseConstants.OpenParentheses);
        hasOpenParentheses = true;
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
            .Append(Constants.Space)
            .Append(useLowerCaseClauses ? ClauseConstants.Where.OrSeparatorLower : ClauseConstants.Where.OrSeparatorUpper)
            .Append(Constants.Space);

        if (!isFilter)
        {
            return;
        }

        stringBuilder.Append(ClauseConstants.OpenParentheses);
        hasOpenParentheses = true;
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
        stringBuilder.Append(Constants.Space);

        switch (clauseAction)
        {
            case ClauseAction.WhereWithFilter:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.AndSeparatorLower : ClauseConstants.Where.AndSeparatorUpper);
                break;

            case ClauseAction.WhereWithOrFilter:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Where.OrSeparatorLower : ClauseConstants.Where.OrSeparatorUpper);
                break;
        }

        stringBuilder.Append(Constants.Space);
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
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.InnerJoinLower : ClauseConstants.Join.InnerJoinUpper);
                break;

            case ClauseAction.LeftJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.LeftJoinLower : ClauseConstants.Join.LeftJoinUpper);
                break;

            case ClauseAction.RightJoin:
                stringBuilder.Append(useLowerCaseClauses ? ClauseConstants.Join.RightJoinLower : ClauseConstants.Join.RightJoinUpper);
                break;
        }

        stringBuilder.Append(Constants.Space);
    }

    private void AppendGroupBy()
    {
        if (clauseActions.Contains(ClauseAction.GroupBy))
        {
            stringBuilder
                .Append(ClauseConstants.GroupBy.Separator)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.GroupBy);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.GroupBy.Lower : ClauseConstants.GroupBy.Upper)
            .Append(Constants.Space);
    }

    private void AppendOrderBy()
    {
        if (clauseActions.Contains(ClauseAction.OrderBy))
        {
            stringBuilder
                .Append(ClauseConstants.OrderBy.Separator)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.OrderBy);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.OrderBy.Lower : ClauseConstants.OrderBy.Upper)
            .Append(Constants.Space);
    }

    private void AppendHaving()
    {
        if (clauseActions.Contains(ClauseAction.Having))
        {
            stringBuilder
                .Append(Constants.Space)
                .Append(useLowerCaseClauses ? ClauseConstants.Having.SeparatorLower : ClauseConstants.Having.SeparatorUpper)
                .Append(Constants.Space);

            return;
        }

        clauseActions.Add(ClauseAction.Having);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Having.Lower : ClauseConstants.Having.Upper)
            .Append(Constants.Space);
    }

    private void AppendOffset()
    {
        if (clauseActions.Contains(ClauseAction.Offset))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Offset);

        if (clauseActions.Contains(ClauseAction.Limit))
        {
            stringBuilder.Append(Constants.Space);
        }
        else
        {
            stringBuilder.AppendLine();
        }

        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Offset.Lower : ClauseConstants.Offset.Upper)
            .Append(Constants.Space);
    }

    private void AppendLimit()
    {
        if (clauseActions.Contains(ClauseAction.Limit))
        {
            return;
        }

        clauseActions.Add(ClauseAction.Limit);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Limit.Lower : ClauseConstants.Limit.Upper)
            .Append(Constants.Space);
    }

    private void AppendFetchNext()
    {
        if (clauseActions.Contains(ClauseAction.FetchNext))
        {
            return;
        }

        clauseActions.Add(ClauseAction.FetchNext);
        stringBuilder
            .AppendLine()
            .Append(useLowerCaseClauses ? ClauseConstants.Fetch.NextLower : ClauseConstants.Fetch.NextUpper)
            .Append(Constants.Space);
    }

    private void AppendRows(bool appendTrailingSpace = true)
    {
        if (!clauseActions.Contains(ClauseAction.Rows))
        {
            clauseActions.Add(ClauseAction.Rows);
        }

        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Rows.Lower : ClauseConstants.Rows.Upper);

        if (!appendTrailingSpace)
        {
            return;
        }

        stringBuilder.Append(Constants.Space);
    }

    private void AppendOnly()
    {
        if (!clauseActions.Contains(ClauseAction.Only))
        {
            clauseActions.Add(ClauseAction.Only);
        }

        stringBuilder
            .Append(useLowerCaseClauses ? ClauseConstants.Only.Lower : ClauseConstants.Only.Upper);
    }

    private bool CanAppendClause(ClauseAction clauseAction)
    {
        return !clauseActions.Exists(c => c is ClauseAction.Delete or ClauseAction.Update)
            || clauseAction is not ClauseAction.GroupBy and not ClauseAction.Having and not ClauseAction.OrderBy and not ClauseAction.FetchNext
               and not ClauseAction.Limit and not ClauseAction.Offset and not ClauseAction.Rows and not ClauseAction.Only;
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
