namespace Dapper.SimpleSqlBuilder;

public interface ISelectBuilder
{
#if NET6_0_OR_GREATER
    ISelectBuilder Select([InterpolatedStringHandlerArgument("")] ref SelectInterpolatedStringHandler handler);

    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    ISelectBuilder Select(FormattableString formattable);

    ISelectFromBuilder From(FormattableString formattable);

#endif
}

public interface ISelectDistinctBuilder
{
#if NET6_0_OR_GREATER
    ISelectDistinctBuilder SelectDistinct([InterpolatedStringHandlerArgument("")] ref SelectDistinctInterpolatedStringHandler handler);

    ISelectFromBuilder From([InterpolatedStringHandlerArgument("")] ref SelectFromInterpolatedStringHandler handler);
#else

    ISelectDistinctBuilder SelectDistinct(FormattableString formattable);

    ISelectFromBuilder From(FormattableString formattable);

#endif
}

public interface ISelectFromBuilder : IJoinBuilder
{
}

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
