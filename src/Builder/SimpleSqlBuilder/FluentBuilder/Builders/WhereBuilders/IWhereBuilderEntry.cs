namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IWhereBuilderEntry : IFluentSqlBuilder
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
