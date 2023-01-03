namespace Dapper.SimpleSqlBuilder.FluentBuilder;

public interface IJoinBuilder : IWhereBuilderEntry
{
#if NET6_0_OR_GREATER
    IJoinBuilder InnerJoin([InterpolatedStringHandlerArgument("")] ref InnerJoinInterpolatedStringHandler handler);

    IJoinBuilder InnerJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref InnerJoinInterpolatedStringHandler handler);

    IJoinBuilder LeftJoin([InterpolatedStringHandlerArgument("")] ref LeftJoinInterpolatedStringHandler handler);

    IJoinBuilder LeftJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref LeftJoinInterpolatedStringHandler handler);

    IJoinBuilder RightJoin([InterpolatedStringHandlerArgument("")] ref RightJoinInterpolatedStringHandler handler);

    IJoinBuilder RightJoin(bool condition, [InterpolatedStringHandlerArgument("condition", "")] ref RightJoinInterpolatedStringHandler handler);
#else

    IJoinBuilder InnerJoin(FormattableString formattable);

    IJoinBuilder InnerJoin(bool condition, FormattableString formattable);

    IJoinBuilder LeftJoin(FormattableString formattable);

    IJoinBuilder LeftJoin(bool condition, FormattableString formattable);

    IJoinBuilder RightJoin(FormattableString formattable);

    IJoinBuilder RightJoin(bool condition, FormattableString formattable);

#endif
}
