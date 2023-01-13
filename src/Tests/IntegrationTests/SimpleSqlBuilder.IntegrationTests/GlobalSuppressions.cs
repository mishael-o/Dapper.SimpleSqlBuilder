// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MSSql.MSSqlTestsFixture.CreateSqlServerContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.MySql.MySqlTestsFixture.CreateMySQLContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed", Scope = "member", Target = "~M:Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql.PostgreSqlTestsFixture.CreatePostgreSQLContainer(System.String)~DotNet.Testcontainers.Containers.TestcontainersContainer")]
