namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IUpdateBuilder : IWhereBuilderEntry
{
#if NET6_0_OR_GREATER
    IUpdateBuilder Set([InterpolatedStringHandlerArgument("")] ref UpdateSetInterpolatedStringHandler handler);

    IUpdateBuilder Set(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref UpdateSetInterpolatedStringHandler handler);
#else

    IUpdateBuilder Set(FormattableString formattable);

    IUpdateBuilder Set(bool condition, FormattableString formattable);

#endif
}

public interface IUpdateBuilderEntry
{
#if NET6_0_OR_GREATER
    IUpdateBuilder Update([InterpolatedStringHandlerArgument("")] ref UpdateInterpolatedStringHandler handler);
#else

    IUpdateBuilder Update(FormattableString formattable);

#endif
}
