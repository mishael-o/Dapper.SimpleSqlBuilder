namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IInsertBuilder : IInsertValueBuilder
{
#if NET6_0_OR_GREATER
    IInsertBuilder Columns([InterpolatedStringHandlerArgument("")] ref InsertColumnInterpolatedStringHandler handler);
#else

    IInsertBuilder Columns(FormattableString formattable);

#endif
}

public interface IInsertValueBuilder : IFluentSqlBuilder
{
#if NET6_0_OR_GREATER
    IInsertValueBuilder Values([InterpolatedStringHandlerArgument("")] ref InsertValueInterpolatedStringHandler handler);
#else

    IInsertValueBuilder Values(FormattableString formattable);

#endif
}

public interface IInsertBuilderEntry
{
#if NET6_0_OR_GREATER
    IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler);
#else

    IInsertBuilder InsertInto(FormattableString formattable);

#endif
}
