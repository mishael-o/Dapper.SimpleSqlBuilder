namespace Dapper.SimpleSqlBuilder;

public interface IDeleteBuilder : IWhereBuilderEntry
{
}

public interface IDeleteBuilderEntry
{
#if NET6_0_OR_GREATER
    IDeleteBuilder DeleteFrom([InterpolatedStringHandlerArgument("")] ref DeleteInterpolatedStringHandler handler);
#else

    IDeleteBuilder DeleteFrom(FormattableString formattable);

#endif
}
