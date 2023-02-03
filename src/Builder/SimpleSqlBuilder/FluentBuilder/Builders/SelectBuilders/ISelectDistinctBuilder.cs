namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectDistinctBuilder : ISelectFromBuilderEntry
{
#if NET6_0_OR_GREATER
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);
#else

    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);
#endif
}
