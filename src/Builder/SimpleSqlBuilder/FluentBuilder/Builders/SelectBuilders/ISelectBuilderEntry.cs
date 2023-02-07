namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface ISelectBuilderEntry
{
#if NET6_0_OR_GREATER
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);
#else

    ISelectBuilder Select(FormattableString formattable);

    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

#endif
}
