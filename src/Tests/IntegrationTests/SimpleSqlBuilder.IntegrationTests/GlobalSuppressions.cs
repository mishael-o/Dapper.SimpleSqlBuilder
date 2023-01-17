// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MSSql.MSSqlTestsFixture.CreateSqlServerContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MySql.MySqlTestsFixture.CreateMySQLContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql.PostgreSqlTestsFixture.CreatePostgreSQLContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.MySql.MySqlTestsCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.MSSql.MSSqlTestsCollection")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Reviewed", Scope = "type", Target = "~T:Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql.PostgreSqlTestsCollection")]
