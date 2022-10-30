namespace Dapper.SimpleSqlBuilder;

public interface IWhereBuilder : IWhereBuilderEntry
{
#if NET6_0_OR_GREATER
    IWhereBuilder Or([InterpolatedStringHandlerArgument("")] ref WhereOrInterpolatedStringHandler handler);

    IWhereBuilder Or(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereOrInterpolatedStringHandler handler);
#else

    IWhereBuilder Or(FormattableString formattable);

    IWhereBuilder Or(bool condition, FormattableString formattable);

#endif
}

public interface IWhereBuilderEntry : IFluentBuilder
{
#if NET6_0_OR_GREATER
    IWhereBuilder Where([InterpolatedStringHandlerArgument("")] ref WhereInterpolatedStringHandler handler);

    IWhereBuilder Where(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref WhereInterpolatedStringHandler handler);
#else

    IWhereBuilder Where(FormattableString formattable);

    IWhereBuilder Where(bool condition, FormattableString formattable);

#endif
}
