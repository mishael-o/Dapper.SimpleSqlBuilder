namespace Dapper.SimpleSqlBuilder;

internal enum Clause
{
    None,
    Delete,
    GroupBy,
    Having,
    InnerJoin,
    Insert,
    InsertValue,
    LeftJoin,
    OrderBy,
    RightJoin,
    Select,
    SelectDistinct,
    SelectFrom,
    Update,
    UpdateSet,
    Where,
    WhereOr
}
