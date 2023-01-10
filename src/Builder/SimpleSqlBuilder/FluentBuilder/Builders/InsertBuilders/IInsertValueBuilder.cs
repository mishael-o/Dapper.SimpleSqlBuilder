namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IInsertValueBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    IInsertValueBuilder Values([InterpolatedStringHandlerArgument("")] ref InsertValueInterpolatedStringHandler handler);
#else

    IInsertValueBuilder Values(FormattableString formattable);

#endif
}
