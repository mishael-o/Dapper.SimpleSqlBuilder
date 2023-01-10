namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IHavingBuilder : IOrderByBuilder
{
#if NET6_0_OR_GREATER
    IHavingBuilder Having([InterpolatedStringHandlerArgument("")] ref HavingInterpolatedStringHandler handler);

    IHavingBuilder Having(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref HavingInterpolatedStringHandler handler);
#else

    IHavingBuilder Having(FormattableString formattable);

    IHavingBuilder Having(bool condition, FormattableString formattable);

#endif
}
