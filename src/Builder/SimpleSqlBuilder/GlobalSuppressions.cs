// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IWhereBuilder")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.ISelectBuilder")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.ISelectBuilderEntry")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IUpdateBuilder")]

#if NET6_0_OR_GREATER
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.DeleteInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.GroupByInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.HavingInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.InnerJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.InsertInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.InsertValueInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.LeftJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.OrderByInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.RightJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.SelectDistinctInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.SelectFromInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.SelectInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.UpdateInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.UpdateSetInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.WhereInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.WhereOrInterpolatedStringHandler")]
#endif
