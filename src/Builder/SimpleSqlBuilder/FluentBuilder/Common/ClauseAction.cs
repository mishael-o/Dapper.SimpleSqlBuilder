namespace Dapper.SimpleSqlBuilder;

internal enum ClauseAction
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
    WhereOr,
    WhereFilter,
    WhereOrFilter,
    WhereWithFilter,
    WhereWithOrFilter
}
