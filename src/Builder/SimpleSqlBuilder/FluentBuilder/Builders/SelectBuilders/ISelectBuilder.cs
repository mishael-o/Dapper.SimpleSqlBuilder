namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectBuilder : ISelectFromBuilderEntry
{
#if NET6_0_OR_GREATER
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

#else
    ISelectBuilder Select(FormattableString formattable);

#endif
}
