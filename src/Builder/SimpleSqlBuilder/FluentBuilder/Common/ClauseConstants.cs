namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal static class ClauseConstants
{
    private const char Comma = ',';
    private const string AndLower = "and";

    internal const char OpenParentheses = '(';
    internal const char CloseParentheses = ')';

    internal static class Delete
    {
        internal const string Lower = "delete from";
        internal static readonly string Upper = Lower.ToUpperInvariant();
    }

    internal static class GroupBy
    {
        internal const char Separator = Comma;
        internal const string Lower = "group by";
        internal static readonly string Upper = Lower.ToUpperInvariant();
    }

    internal static class Having
    {
        internal const string Lower = "having";
        internal const string SeparatorLower = AndLower;
        internal static readonly string SeparatorUpper = AndLower.ToUpperInvariant();
        internal static readonly string Upper = Lower.ToUpperInvariant();
    }

    internal static class Insert
    {
        internal const string Lower = "insert into";
        internal const string ValuesLower = "values";
        internal const char Separator = Comma;
        internal static readonly string Upper = Lower.ToUpperInvariant();
        internal static readonly string ValuesUpper = ValuesLower.ToUpperInvariant();
    }

    internal static class Join
    {
        internal const string InnerJoinLower = "inner join";
        internal const string LeftJoinLower = "left join";
        internal const string RightJoinLower = "right join";
        internal static readonly string InnerJoinUpper = InnerJoinLower.ToUpperInvariant();
        internal static readonly string LeftJoinUpper = LeftJoinLower.ToUpperInvariant();
        internal static readonly string RightJoinUpper = RightJoinLower.ToUpperInvariant();
    }

    internal static class OrderBy
    {
        internal const char Separator = Comma;
        internal const string Lower = "order by";
        internal static readonly string Upper = Lower.ToUpperInvariant();
    }

    internal static class Select
    {
        internal const string Lower = "select";
        internal const string DistinctLower = "select distinct";
        internal const string FromLower = "from";
        internal const char Separator = Comma;
        internal static readonly string Upper = Lower.ToUpperInvariant();
        internal static readonly string DistinctUpper = DistinctLower.ToUpperInvariant();
        internal static readonly string FromUpper = FromLower.ToUpperInvariant();
    }

    internal static class Update
    {
        internal const string Lower = "update";
        internal const string SetLower = "set";
        internal const char SetSeparator = Comma;
        internal static readonly string Upper = Lower.ToUpperInvariant();
        internal static readonly string SetUpper = SetLower.ToUpperInvariant();
    }

    internal static class Where
    {
        internal const string Lower = "where";
        internal const string OrSeparatorLower = "or";
        internal const string AndSeparatorLower = AndLower;
        internal static readonly string Upper = Lower.ToUpperInvariant();
        internal static readonly string AndSeparatorUpper = AndSeparatorLower.ToUpperInvariant();
        internal static readonly string OrSeparatorUpper = OrSeparatorLower.ToUpperInvariant();
    }
}
