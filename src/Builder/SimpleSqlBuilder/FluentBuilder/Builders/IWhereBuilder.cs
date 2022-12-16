namespace Dapper.SimpleSqlBuilder;

public interface IWhereBuilder : IWhereBuilderEntry, IGroupByBuilder
{
#if NET6_0_OR_GREATER
    IWhereBuilder OrWhere([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler);

    IWhereBuilder OrWhere(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereOrInterpolatedStringHandler handler);

    IWhereFilter OrWhereFilter([InterpolatedStringHandlerArgument("")] ref WhereOrFilterInterpolatedStringHandler handler);

#else

    IWhereBuilder OrWhere(FormattableString formattable);

    IWhereBuilder OrWhere(bool condition, FormattableString formattable);

    IWhereFilter OrWhereFilter(FormattableString formattable);

#endif

    IWhereFilterEntry OrWhereFilter();
}

public interface IWhereBuilderEntry : IFluentBuilder
{
#if NET6_0_OR_GREATER
    IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler);

    IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler);

    IWhereFilter WhereFilter([InterpolatedStringHandlerArgument("")] ref WhereFilterInterpolatedStringHandler handler);
#else

    IWhereBuilder Where(FormattableString formattable);

    IWhereBuilder Where(bool condition, FormattableString formattable);

    IWhereFilter WhereFilter(FormattableString formattable);

#endif

    IWhereFilterEntry WhereFilter();
}

public interface IWhereFilterEntry : IFluentBuilder
{
#if NET6_0_OR_GREATER
    IWhereFilter WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler);

    IWhereFilter WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler);
#else

    IWhereFilter WithFilter(FormattableString formattable);

    IWhereFilter WithFilter(bool condition, FormattableString formattable);

#endif
}

public interface IWhereFilter : IWhereFilterEntry, IWhereBuilder
{
#if NET6_0_OR_GREATER
    IWhereFilter WithOrFilter([InterpolatedStringHandlerArgument("")] ref WhereWithOrFilterInterpolatedStringHandler handler);

    IWhereFilter WithOrFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithOrFilterInterpolatedStringHandler handler);
#else

    IWhereFilter WithOrFilter(FormattableString formattable);

    IWhereFilter WithOrFilter(bool condition, FormattableString formattable);

#endif
}
