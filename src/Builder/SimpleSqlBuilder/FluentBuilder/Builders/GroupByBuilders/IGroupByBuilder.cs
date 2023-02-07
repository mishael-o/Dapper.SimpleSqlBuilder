namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IGroupByBuilder : IHavingBuilder
{
#if NET6_0_OR_GREATER
    IGroupByBuilder GroupBy([InterpolatedStringHandlerArgument("")] ref GroupByInterpolatedStringHandler handler);

    IGroupByBuilder GroupBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref GroupByInterpolatedStringHandler handler);
#else

    IGroupByBuilder GroupBy(FormattableString formattable);

    IGroupByBuilder GroupBy(bool condition, FormattableString formattable);

#endif
}
