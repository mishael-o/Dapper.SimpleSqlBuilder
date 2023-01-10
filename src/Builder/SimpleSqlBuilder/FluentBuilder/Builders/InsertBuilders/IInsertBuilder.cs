namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IInsertBuilder : IInsertValueBuilder
{
#if NET6_0_OR_GREATER
    IInsertBuilder Columns([InterpolatedStringHandlerArgument("")] ref InsertColumnInterpolatedStringHandler handler);
#else

    IInsertBuilder Columns(FormattableString formattable);

#endif
}
