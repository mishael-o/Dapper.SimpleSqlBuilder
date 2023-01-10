namespace Dapper.SimpleSqlBuilder.FluentBuilder;

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
