namespace Dapper.SimpleSqlBuilder;

internal static class ClauseConstants
{
    private const string CommaSeperator = ", ";

    internal static class Delete
    {
        internal const string Upper = "DELETE FROM ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class GroupBy
    {
        internal const string Seperator = CommaSeperator;
        internal static readonly string Upper = Environment.NewLine + "GROUP BY ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Having
    {
        internal const string Seperator = "AND ";
        internal static readonly string Upper = Environment.NewLine + "HAVING ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Insert
    {
        internal const string Upper = "INSERT INTO ";
        internal const char Opener = '(';
        internal const char Closer = ')';
        internal const string ValuesSeperator = CommaSeperator;
        internal static readonly string Values = Environment.NewLine + "VALUES " + Opener;
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string ValuesLower = Values.ToLowerInvariant();
    }

    internal static class Join
    {
        internal static readonly string UpperInnerJoin = Environment.NewLine + "INNER JOIN ";
        internal static readonly string UpperLeftJoin = Environment.NewLine + "LEFT JOIN ";
        internal static readonly string UpperRightJoin = Environment.NewLine + "RIGHT JOIN ";
        internal static readonly string LowerInnerJoin = UpperInnerJoin.ToLowerInvariant();
        internal static readonly string LowerLeftJoin = UpperLeftJoin.ToUpperInvariant();
        internal static readonly string LowerRightJoin = UpperRightJoin.ToUpperInvariant();
    }

    internal static class OrderBy
    {
        internal const string Seperator = CommaSeperator;
        internal static readonly string Upper = Environment.NewLine + "ORDER BY ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
    }

    internal static class Select
    {
        internal const string Upper = "SELECT ";
        internal const string DistinctUpper = "SELECT DISTINCT ";
        internal const string Seperator = CommaSeperator;
        internal static readonly string FromUpper = Environment.NewLine + "FROM ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string DistinctLower = DistinctUpper.ToLowerInvariant();
        internal static readonly string FromLower = FromUpper.ToLowerInvariant();
    }

    internal static class Update
    {
        internal const string Upper = "UPDATE ";
        internal const string SetSeperator = CommaSeperator;
        internal static readonly string SetUpper = Environment.NewLine + "SET ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string SetLower = Upper.ToLowerInvariant();
    }

    internal static class Where
    {
        internal const string AndSeperator = "AND ";
        internal const string OrSeperator = "OR ";
        internal static readonly string Upper = Environment.NewLine + "WHERE ";
        internal static readonly string Lower = Upper.ToLowerInvariant();
        internal static readonly string AndSeperatorLower = AndSeperator.ToLowerInvariant();
        internal static readonly string OrSeperatorLower = OrSeperator.ToLowerInvariant();
    }
}
