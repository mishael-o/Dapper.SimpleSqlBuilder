namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectFromBuilderEntry : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    ISelectFromBuilder From(FormattableString formattable);
#endif
}
