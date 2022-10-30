using System.Data;
using System.Text;

namespace Dapper.SimpleSqlBuilder;

/// <summary>
/// Core <see cref="SimpleFluentBuilder"/> partial class.
/// </summary>
internal partial class SimpleFluentBuilder : ISimpleFluentBuilder
{
    private readonly string parameterNameTemplate;
    private readonly string parameterPrefix;
    private readonly bool reuseParameters;
    private readonly bool useLowerCaseClauses;

    private readonly StringBuilder stringBuilder;
    private readonly DynamicParameters parameters;
    private readonly List<Clause> clauses;

    public SimpleFluentBuilder(string parameterNameTemplate, string parameterPrefix, bool reuseParameters, bool useLowerCaseClauses)
    {
        this.parameterNameTemplate = parameterNameTemplate;
        this.parameterPrefix = parameterPrefix;
        this.reuseParameters = reuseParameters;
        this.useLowerCaseClauses = useLowerCaseClauses;

        stringBuilder = new();
        parameters = new();
        clauses = new();
    }

    public string Sql
        => GetSql();

    public object Parameters
        => parameters;

    public IEnumerable<string> ParameterNames
        => parameters.ParameterNames;

#if NET6_0_OR_GREATER
    public IDeleteBuilder DeleteFrom([InterpolatedStringHandlerArgument("")] ref DeleteInterpolatedStringHandler handler)
        => this;

    public IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler)
        => this;

    public IInsertBuilder Values(ref InsertValueInterpolatedStringHandler handler)
        => this;

    public ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler)
        => this;

    public ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler)
        => this;

    public ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler)
        => this;

    public IWhereBuilder Or([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler)
        => this;

    public IWhereBuilder Or(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref WhereOrInterpolatedStringHandler handler)
        => this;

    public IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler)
        => this;

    public IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref WhereInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder InnerJoin(ref InnerJoinInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref InnerJoinInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder LeftJoin(ref LeftJoinInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref LeftJoinInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder RightJoin(ref RightJoinInterpolatedStringHandler handler)
        => this;

    public IJoinBuilder RightJoin(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref RightJoinInterpolatedStringHandler handler)
        => this;

    public IOrderByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler)
        => this;

    public IOrderByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref GroupByInterpolatedStringHandler handler)
        => this;

    public IHavingBuilder Having([InterpolatedStringHandlerArgument("")] ref HavingInterpolatedStringHandler handler)
        => this;

    public IHavingBuilder Having(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref HavingInterpolatedStringHandler handler)
        => this;

    public IOrderByBuilder OrderBy([InterpolatedStringHandlerArgument("")] ref OrderByInterpolatedStringHandler handler)
        => this;

    public IOrderByBuilder OrderBy(bool condition, [InterpolatedStringHandlerArgument(new[] { "condition", "" })] ref OrderByInterpolatedStringHandler handler)
        => this;
#else

    public IDeleteBuilder DeleteFrom(FormattableString formattable)
    {
        FormatValue(formattable, Clause.Delete);
        return this;
    }

    public IInsertBuilder InsertInto(FormattableString formattable)
    {
        FormatValue(formattable, Clause.Insert);
        return this;
    }

    public IInsertBuilder Values(FormattableString formattable)
    {
        FormatValue(formattable, Clause.InsertValue);
        return this;
    }

    public ISelectBuilder Select(FormattableString formattable)
    {
        FormatValue(formattable, Clause.Select);
        return this;
    }

    public ISelectDistinctBuilder SelectDistinct(FormattableString formattable)
    {
        FormatValue(formattable, Clause.SelectDistinct);
        return this;
    }

    public ISelectFromBuilder From(FormattableString formattable)
    {
        FormatValue(formattable, Clause.SelectFrom);
        return this;
    }

    public IWhereBuilder Or(FormattableString formattable)
    {
        FormatValue(formattable, Clause.WhereOr);
        return this;
    }

    public IWhereBuilder Or(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.WhereOr);
        }

        return this;
    }

    public IJoinBuilder InnerJoin(FormattableString formattable)
    {
        FormatValue(formattable, Clause.InnerJoin);
        return this;
    }

    public IJoinBuilder InnerJoin(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.InnerJoin);
        }

        return this;
    }

    public IJoinBuilder LeftJoin(FormattableString formattable)
    {
        FormatValue(formattable, Clause.LeftJoin);
        return this;
    }

    public IJoinBuilder LeftJoin(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.LeftJoin);
        }

        return this;
    }

    public IJoinBuilder RightJoin(FormattableString formattable)
    {
        FormatValue(formattable, Clause.RightJoin);
        return this;
    }

    public IJoinBuilder RightJoin(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.RightJoin);
        }

        return this;
    }

    public IGroupByBuilder GroupBy(FormattableString formattable)
    {
        FormatValue(formattable, Clause.GroupBy);
        return this;
    }

    public IGroupByBuilder GroupBy(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.GroupBy);
        }

        return this;
    }

    public IHavingBuilder Having(FormattableString formattable)
    {
        FormatValue(formattable, Clause.Having);
        return this;
    }

    public IHavingBuilder Having(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.Having);
        }

        return this;
    }

    public IOrderByBuilder OrderBy(FormattableString formattable)
    {
        FormatValue(formattable, Clause.OrderBy);
        return this;
    }

    public IOrderByBuilder OrderBy(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.OrderBy);
        }

        return this;
    }

    public IWhereBuilder Where(FormattableString formattable)
    {
        FormatValue(formattable, Clause.Where);
        return this;
    }

    public IWhereBuilder Where(bool condition, FormattableString formattable)
    {
        if (condition)
        {
            FormatValue(formattable, Clause.Where);
        }

        return this;
    }

#endif

    public IFluentBuilder AddParameter(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
    {
        parameters.Add(name, value, dbType, direction, size, precision, scale);
        return this;
    }

    public T GetValue<T>(string parameterName)
        => parameters.Get<T>(parameterName);
}
