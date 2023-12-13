// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.MySql.MySqlTestsCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.MSSql.MSSqlTestsCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql.PostgreSqlTestsCollection")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "Suppressing this until StyleCop Analyzers supports this", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MSSql.MSSqlTestsFixture.InitialiseRespawnerAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "Suppressing this until StyleCop Analyzers supports this", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MySql.MySqlTestsFixture.InitialiseRespawnerAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "Suppressing this until StyleCop Analyzers supports this", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql.PostgreSqlTestsFixture.InitialiseRespawnerAsync~System.Threading.Tasks.Task")]
