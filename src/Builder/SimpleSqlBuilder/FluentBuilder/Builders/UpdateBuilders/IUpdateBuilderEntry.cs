namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IUpdateBuilderEntry
{
#if NET6_0_OR_GREATER
    IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler);
#else

    IUpdateBuilder Update(FormattableString formattable);

#endif
}
