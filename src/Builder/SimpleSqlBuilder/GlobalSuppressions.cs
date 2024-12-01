// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereBuilder")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.ISelectBuilder")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.ISelectBuilderEntry")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.IUpdateBuilder")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.IFluentBuilder")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "A flattened namespace is used for all types for improved DX for consumers.", Scope = "namespaceanddescendants", Target = "~N:Dapper.SimpleSqlBuilder")]
#if NET6_0_OR_GREATER
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.AppendInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.AppendIntactInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.AppendNewLineInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.DeleteInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.GroupByInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.HavingInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.InnerJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.InsertInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.InsertColumnInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.InsertValueInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.LeftJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.OrderByInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.RightJoinInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.SelectDistinctInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.SelectFromInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.SelectInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.UpdateInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.UpdateSetInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereOrInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereFilterInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereOrFilterInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithFilterInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithOrFilterInterpolatedStringHandler")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.BuilderInterpolatedStringHandler")]
#endif
