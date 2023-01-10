namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    ISelectBuilder Select(FormattableString formattable);

    ISelectFromBuilder From(FormattableString formattable);

#endif
}
