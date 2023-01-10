namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal static class ClauseConstants
{
    private const char Comma = ',';
    private const string And = "AND";

    internal const char Space = ' ';
    internal const char OpenParentheses = '(';
    internal const char CloseParentheses = ')';

    internal static class Delete
    {
        internal const string Upper = "DELETE FROM";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class GroupBy
    {
        internal const char Seperator = Comma;
        internal const string Upper = "GROUP BY";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Having
    {
        internal const string Seperator = And;
        internal const string Upper = "HAVING";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Insert
    {
        internal const string Upper = "INSERT INTO";
        internal const string Values = "VALUES";
        internal const char Seperator = Comma;
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string ValuesLower = Values.ToLowerInvariant();
    }

    internal static class Join
    {
        internal const string UpperInnerJoin = "INNER JOIN";
        internal const string UpperLeftJoin = "LEFT JOIN";
        internal const string UpperRightJoin = "RIGHT JOIN";
        internal static readonly string LowerInnerJoin = UpperInnerJoin.ToLowerInvariant();
        internal static readonly string LowerLeftJoin = UpperLeftJoin.ToLowerInvariant();
        internal static readonly string LowerRightJoin = UpperRightJoin.ToLowerInvariant();
    }

    internal static class OrderBy
    {
        internal const char Seperator = Comma;
        internal const string Upper = "ORDER BY";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Select
    {
        internal const string Upper = "SELECT";
        internal const string DistinctUpper = "SELECT DISTINCT";
        internal const string FromUpper = "FROM";
        internal const char Seperator = Comma;
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string DistinctLower = DistinctUpper.ToLowerInvariant();
        internal static readonly string FromLower = FromUpper.ToLowerInvariant();
    }

    internal static class Update
    {
        internal const string Upper = "UPDATE";
        internal const string SetUpper = "SET";
        internal const char SetSeperator = Comma;
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string SetLower = SetUpper.ToLowerInvariant();
    }

    internal static class Where
    {
        internal const string Upper = "WHERE";
        internal const string AndSeperator = And;
        internal const string OrSeperator = "OR";
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string AndSeperatorLower = AndSeperator.ToLowerInvariant();
        internal static readonly string OrSeperatorLower = OrSeperator.ToLowerInvariant();
    }
}
