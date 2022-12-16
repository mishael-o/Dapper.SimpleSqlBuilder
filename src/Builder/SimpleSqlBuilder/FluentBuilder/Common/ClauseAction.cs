namespace Dapper.SimpleSqlBuilder;

internal enum ClauseAction
{
    None,
    Delete,
    GroupBy,
    Having,
    InnerJoin,
    Insert,
    Insert_Value,
    LeftJoin,
    OrderBy,
    RightJoin,
    Select,
    Select_Distinct,
    Select_From,
    Update,
    Update_Set,
    Where,
    Where_Or,
    Where_Filter,
    Where_Or_Filter,
    Where_With_Filter,
    Where_With_Or_Filter
}
