namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal enum ClauseAction
{
    None,
    Delete,
    GroupBy,
    Having,
    InnerJoin,
    Insert,
    InsertColumn,
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
