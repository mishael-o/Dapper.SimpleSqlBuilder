namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectDistinctBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);

    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

    ISelectFromBuilder From(FormattableString formattable);

#endif
}
