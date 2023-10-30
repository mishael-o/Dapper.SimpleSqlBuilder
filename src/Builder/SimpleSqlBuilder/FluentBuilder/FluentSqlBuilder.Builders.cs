using System.Data;

namespace Dapper.SimpleSqlBuilder.FluentBuilder;

/// <summary>
/// Implements <see cref="ISimpleFluentBuilder"/> and <see cref="ISimpleFluentBuilderEntry"/> interfaces for the <see cref="FluentSqlBuilder"/> type.
/// </summary>
internal sealed partial class FluentSqlBuilder : ISimpleFluentBuilder, ISimpleFluentBuilderEntry
{
    public string Sql
        => stringBuilder.ToString();

    public object Parameters
        => sqlFormatter.Parameters;

    public IEnumerable<string> ParameterNames
        => sqlFormatter.Parameters.ParameterNames;

    public void AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
        => sqlFormatter.Parameters.Add(name, value, dbType, direction, size, precision, scale);

    public T GetValue<T>(string parameterName)
        => sqlFormatter.Parameters.Get<T>(parameterName);

#if NET6_0_OR_GREATER
    public IDeleteBuilder DeleteFrom([InterpolatedStringHandlerArgument("")] ref DeleteInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IInsertBuilder Columns([InterpolatedStringHandlerArgument("")] ref InsertColumnInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IInsertValueBuilder Values([InterpolatedStringHandlerArgument("")] ref InsertValueInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IUpdateBuilder Set([InterpolatedStringHandlerArgument("")] ref UpdateSetInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IUpdateBuilder Set(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref UpdateSetInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder InnerJoin([InterpolatedStringHandlerArgument("")] ref InnerJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref InnerJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder LeftJoin([InterpolatedStringHandlerArgument("")] ref LeftJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref LeftJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder RightJoin([InterpolatedStringHandlerArgument("")] ref RightJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder RightJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref RightJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereBuilder OrWhere([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereBuilder OrWhere(bool condition, [InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder OrWhereFilter([InterpolatedStringHandlerArgument("")] ref WhereOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder WithOrFilter([InterpolatedStringHandlerArgument("")] ref WhereWithOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilterBuilder WithOrFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IGroupByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IGroupByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref GroupByInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IHavingBuilder Having([InterpolatedStringHandlerArgument("")] ref HavingInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IHavingBuilder Having(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref HavingInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IOrderByBuilder OrderBy([InterpolatedStringHandlerArgument("")] ref OrderByInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IOrderByBuilder OrderBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref OrderByInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }
#else

    public IDeleteBuilder DeleteFrom(FormattableString formattable)
        => AppendFormattable(ClauseAction.Delete, formattable);

    public IInsertBuilder InsertInto(FormattableString formattable)
        => AppendFormattable(ClauseAction.Insert, formattable);

    public IInsertBuilder Columns(FormattableString formattable)
        => AppendFormattable(ClauseAction.InsertColumn, formattable);

    public IInsertValueBuilder Values(FormattableString formattable)
        => AppendFormattable(ClauseAction.InsertValue, formattable);

    public ISelectBuilder Select(FormattableString formattable)
        => AppendFormattable(ClauseAction.Select, formattable);

    public ISelectDistinctBuilder SelectDistinct(FormattableString formattable)
        => AppendFormattable(ClauseAction.SelectDistinct, formattable);

    public ISelectFromBuilder From(FormattableString formattable)
        => AppendFormattable(ClauseAction.SelectFrom, formattable);

    public IUpdateBuilder Update(FormattableString formattable)
        => AppendFormattable(ClauseAction.Update, formattable);

    public IUpdateBuilder Set(FormattableString formattable)
        => AppendFormattable(ClauseAction.UpdateSet, formattable);

    public IUpdateBuilder Set(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.UpdateSet, formattable, condition);

    public IJoinBuilder InnerJoin(FormattableString formattable)
        => AppendFormattable(ClauseAction.InnerJoin, formattable);

    public IJoinBuilder InnerJoin(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.InnerJoin, formattable, condition);

    public IJoinBuilder LeftJoin(FormattableString formattable)
        => AppendFormattable(ClauseAction.LeftJoin, formattable);

    public IJoinBuilder LeftJoin(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.LeftJoin, formattable, condition);

    public IJoinBuilder RightJoin(FormattableString formattable)
        => AppendFormattable(ClauseAction.RightJoin, formattable);

    public IJoinBuilder RightJoin(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.RightJoin, formattable, condition);

    public IWhereBuilder Where(FormattableString formattable)
        => AppendFormattable(ClauseAction.Where, formattable);

    public IWhereBuilder Where(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.Where, formattable, condition);

    public IWhereBuilder OrWhere(FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereOr, formattable);

    public IWhereBuilder OrWhere(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereOr, formattable, condition);

    public IWhereFilterBuilder WhereFilter(FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereFilter, formattable);

    public IWhereFilterBuilder OrWhereFilter(FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereOrFilter, formattable);

    public IWhereFilterBuilder WithFilter(FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereWithFilter, formattable);

    public IWhereFilterBuilder WithFilter(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereWithFilter, formattable, condition);

    public IWhereFilterBuilder WithOrFilter(FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereWithOrFilter, formattable);

    public IWhereFilterBuilder WithOrFilter(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.WhereWithOrFilter, formattable, condition);

    public IGroupByBuilder GroupBy(FormattableString formattable)
        => AppendFormattable(ClauseAction.GroupBy, formattable);

    public IGroupByBuilder GroupBy(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.GroupBy, formattable, condition);

    public IHavingBuilder Having(FormattableString formattable)
        => AppendFormattable(ClauseAction.Having, formattable);

    public IHavingBuilder Having(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.Having, formattable, condition);

    public IOrderByBuilder OrderBy(FormattableString formattable)
        => AppendFormattable(ClauseAction.OrderBy, formattable);

    public IOrderByBuilder OrderBy(bool condition, FormattableString formattable)
        => AppendFormattable(ClauseAction.OrderBy, formattable, condition);

    private ISimpleFluentBuilder AppendFormattable(ClauseAction clauseAction, FormattableString formattable, bool condition = true)
    {
        if (!condition || !CanAppendClause(clauseAction))
        {
            return this;
        }

        AppendClause(clauseAction);

        if (formattable.ArgumentCount == 0)
        {
            stringBuilder.Append(formattable.Format);
        }
        else
        {
            stringBuilder.AppendFormat(sqlFormatter, formattable.Format, formattable.GetArguments());
        }

        CloseOpenParentheses();
        return this;
    }

#endif

    public IWhereFilterBuilderEntry WhereFilter()
    {
        pendingWhereFilter = ClauseAction.WhereFilter;
        return this;
    }

    public IWhereFilterBuilderEntry OrWhereFilter()
    {
        pendingWhereFilter = ClauseAction.WhereOrFilter;
        return this;
    }

    public IFluentSqlBuilder FetchNext(int rows)
    {
        if (!CanAppendClause(ClauseAction.FetchNext))
        {
            return this;
        }

        AppendFetchNext();
        stringBuilder
            .Append(rows)
            .Append(Constants.Space);
        AppendRows();
        AppendOnly();
        return this;
    }

    public IFetchBuilder OffsetRows(int offset)
    {
        if (!CanAppendClause(ClauseAction.Offset))
        {
            return this;
        }

        AppendOffset();
        stringBuilder
            .Append(offset)
            .Append(Constants.Space);
        AppendRows(false);
        return this;
    }

    public IOffsetBuilder Limit(int rows)
    {
        if (!CanAppendClause(ClauseAction.Limit))
        {
            return this;
        }

        AppendLimit();
        stringBuilder.Append(rows);
        return this;
    }

    public IFluentSqlBuilder Offset(int offset)
    {
        if (!CanAppendClause(ClauseAction.Offset))
        {
            return this;
        }

        AppendOffset();
        stringBuilder.Append(offset);
        return this;
    }
}
