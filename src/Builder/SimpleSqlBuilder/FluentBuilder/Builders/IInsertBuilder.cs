namespace Dapper.SimpleSqlBuilder;

public interface IInsertBuilder : IFluentBuilder
{
#if NET6_0_OR_GREATER
    IInsertBuilder Values(ref InsertValueInterpolatedStringHandler handler);
#else

    IInsertBuilder Values(FormattableString formattable);

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
