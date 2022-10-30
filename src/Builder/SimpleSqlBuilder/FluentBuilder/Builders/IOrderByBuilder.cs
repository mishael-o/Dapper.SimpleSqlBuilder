namespace Dapper.SimpleSqlBuilder;

public interface IOrderByBuilder : IFluentBuilder
{
#if NET6_0_OR_GREATER
    IOrderByBuilder OrderBy([InterpolatedStringHandlerArgument("")] ref OrderByInterpolatedStringHandler handler);

    IOrderByBuilder OrderBy(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref OrderByInterpolatedStringHandler handler);
#else

    IOrderByBuilder OrderBy(FormattableString formattable);

    IOrderByBuilder OrderBy(bool condition, FormattableString formattable);

#endif
}
