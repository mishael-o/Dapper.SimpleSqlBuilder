namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IInsertBuilderEntry
{
#if NET6_0_OR_GREATER
    IInsertBuilder InsertInto([InterpolatedStringHandlerArgument("")] ref InsertInterpolatedStringHandler handler);
#else

    IInsertBuilder InsertInto(FormattableString formattable);

#endif
}
