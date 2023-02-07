namespace Dapper.SimpleSqlBuilder.FluentBuilder;

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
