﻿using System.Data;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// <see cref="ISimpleFluentBuilder"/> implementation for <see cref="SimpleFluentBuilder"/> partial class.
/// </summary>
internal partial class SimpleFluentBuilder : ISimpleFluentBuilder
{
    public string Sql
        => stringBuilder.ToString();

    public object Parameters
        => parameters;

    public IEnumerable<string> ParameterNames
        => parameters.ParameterNames;

    public IFluentBuilder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        parameters.Add(name, value, dbType, direction, size, precision, scale);
        return this;
    }

    public T GetValue<T>(string parameterName)
        => parameters.Get<T>(parameterName);

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

    public IInsertBuilder Values(ref InsertValueInterpolatedStringHandler handler)
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

    public IJoinBuilder InnerJoin(ref InnerJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref InnerJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder LeftJoin(ref LeftJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref LeftJoinInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IJoinBuilder RightJoin(ref RightJoinInterpolatedStringHandler handler)
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

    public IWhereFilter WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler)
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

    public IWhereFilter OrWhereFilter([InterpolatedStringHandlerArgument("")] ref WhereOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilter WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilter WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilter WithOrFilter([InterpolatedStringHandlerArgument("")] ref WhereWithOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IWhereFilter WithOrFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithOrFilterInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IOrderByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler)
    {
        handler.Close();
        return this;
    }

    public IOrderByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref GroupByInterpolatedStringHandler handler)
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
        => FormatFormattable(ClauseAction.Delete, formattable);

    public IInsertBuilder InsertInto(FormattableString formattable)
        => FormatFormattable(ClauseAction.Insert, formattable);

    public IInsertBuilder Values(FormattableString formattable)
        => FormatFormattable(ClauseAction.Insert_Value, formattable);

    public ISelectBuilder Select(FormattableString formattable)
        => FormatFormattable(ClauseAction.Select, formattable);

    public ISelectDistinctBuilder SelectDistinct(FormattableString formattable)
        => FormatFormattable(ClauseAction.Select_Distinct, formattable);

    public ISelectFromBuilder From(FormattableString formattable)
        => FormatFormattable(ClauseAction.Select_From, formattable);

    public IJoinBuilder InnerJoin(FormattableString formattable)
        => FormatFormattable(ClauseAction.InnerJoin, formattable);

    public IJoinBuilder InnerJoin(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.InnerJoin, formattable, condition);

    public IJoinBuilder LeftJoin(FormattableString formattable)
        => FormatFormattable(ClauseAction.LeftJoin, formattable);

    public IJoinBuilder LeftJoin(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.LeftJoin, formattable, condition);

    public IJoinBuilder RightJoin(FormattableString formattable)
        => FormatFormattable(ClauseAction.RightJoin, formattable);

    public IJoinBuilder RightJoin(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.RightJoin, formattable, condition);

    public IWhereBuilder Where(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where, formattable);

    public IWhereBuilder Where(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.Where, formattable, condition);

    public IWhereBuilder OrWhere(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_Or, formattable);

    public IWhereBuilder OrWhere(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_Or, formattable, condition);

    public IWhereFilter WhereFilter(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_Filter, formattable);

    public IWhereFilter OrWhereFilter(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_Or_Filter, formattable);

    public IWhereFilter WithFilter(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_With_Filter, formattable);

    public IWhereFilter WithFilter(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_With_Filter, formattable, condition);

    public IWhereFilter WithOrFilter(FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_With_Or_Filter, formattable);

    public IWhereFilter WithOrFilter(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.Where_With_Or_Filter, formattable, condition);

    public IGroupByBuilder GroupBy(FormattableString formattable)
        => FormatFormattable(ClauseAction.GroupBy, formattable);

    public IGroupByBuilder GroupBy(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.GroupBy, formattable, condition);

    public IHavingBuilder Having(FormattableString formattable)
        => FormatFormattable(ClauseAction.Having, formattable);

    public IHavingBuilder Having(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.Having, formattable, condition);

    public IOrderByBuilder OrderBy(FormattableString formattable)
        => FormatFormattable(ClauseAction.OrderBy, formattable);

    public IOrderByBuilder OrderBy(bool condition, FormattableString formattable)
        => FormatFormattable(ClauseAction.OrderBy, formattable, condition);

    private ISimpleFluentBuilder FormatFormattable(ClauseAction clauseAction, FormattableString formattable, bool condition = true)
    {
        if (!condition || !IsClauseActionEnabled(clauseAction))
        {
            return this;
        }

        AppendClause(clauseAction);
        stringBuilder.Append(Format(formattable));
        CloseOpenParentheses();

        return this;
    }

#endif

    public IWhereFilterEntry WhereFilter()
    {
        pendingWhereFilter = ClauseAction.Where_Filter;
        return this;
    }

    public IWhereFilterEntry OrWhereFilter()
    {
        pendingWhereFilter = ClauseAction.Where_Or_Filter;
        return this;
    }
}
