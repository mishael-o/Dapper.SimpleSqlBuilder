namespace Dapper.SimpleSqlBuilder.FluentBuilder;

internal enum ClauseAction
{
    None,
    Delete,
    FetchNext,
    GroupBy,
    Having,
    InnerJoin,
    Insert,
    InsertColumn,
    InsertValue,
    Limit,
    LeftJoin,
    Offset,
    Only,
    OrderBy,
    RightJoin,
    Rows,
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
