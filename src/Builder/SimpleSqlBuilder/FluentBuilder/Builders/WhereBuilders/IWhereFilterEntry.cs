namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IWhereFilterEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    IWhereFilter WithFilter([InterpolatedStringHandlerArgument("")] ref WhereWithFilterInterpolatedStringHandler handler);

    IWhereFilter WithFilter(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereWithFilterInterpolatedStringHandler handler);
#else

    IWhereFilter WithFilter(FormattableString formattable);

    IWhereFilter WithFilter(bool condition, FormattableString formattable);

#endif
}
