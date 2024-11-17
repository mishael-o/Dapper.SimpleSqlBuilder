// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Maintainability", "CA1510:Use ArgumentNullException throw helper", Justification = "Supressing this until we drop .Net Standard 2.1 support", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.DependencyInjection.ServiceCollectionExtensions.AddSimpleSqlBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{Dapper.SimpleSqlBuilder.DependencyInjection.SimpleBuilderOptions},Microsoft.Extensions.DependencyInjection.ServiceLifetime)~Microsoft.Extensions.DependencyInjection.IServiceCollection")]
[assembly: SuppressMessage("Maintainability", "CA1510:Use ArgumentNullException throw helper", Justification = "Supressing this until we drop .Net Standard 2.1 support", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.DependencyInjection.ServiceCollectionExtensions.AddSimpleSqlBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.String,Microsoft.Extensions.DependencyInjection.ServiceLifetime)~Microsoft.Extensions.DependencyInjection.IServiceCollection")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "A flattened namespace is used for all types for improved DX for consumers.", Scope = "namespaceanddescendants", Target = "~N:Dapper.SimpleSqlBuilder.DependencyInjection")]
#if NET6_0_OR_GREATER
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.DependencyInjection.BuilderFactoryInterpolatedStringHandler")]
#endif
