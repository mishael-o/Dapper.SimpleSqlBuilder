namespace Dapper.SimpleSqlBuilder;

public interface IGroupByBuilder : IHavingBuilder
{
#if NET6_0_OR_GREATER
    IOrderByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler);

    IOrderByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref GroupByInterpolatedStringHandler handler);
#else

    IGroupByBuilder GroupBy(FormattableString formattable);

    IGroupByBuilder GroupBy(bool condition, FormattableString formattable);

#endif
}
