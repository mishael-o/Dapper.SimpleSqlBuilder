# Comparison to Dapper

The code snippets below illustrate how this library compares to Dapper and Dapper's SqlBuilder in building SQL queries. For performance comparisons, refer to the [Performance](../miscellaneous/performance.md) section.

The examples demonstrate extracting data from a `Users` table where `UserTypeId` equals a specified `userTypeId` and `Role` matches a specified `role`.

## [Dapper](#tab/dapper)

```csharp
using Dapper;

var sql = @"
SELECT * FROM Users
WHERE UserTypeId = @userTypeId
AND Role = @role";

using var connection = new SqliteConnection("Data Source=database.db");
var users = connection.Query<User>(sql, new { userTypeId, role })
```

---

## [SqlBuilder (Dapper)](#tab/dapper-sqlbuilder)

```csharp
using Dapper;

var sqlBuilder = new SqlBuilder()
    .Where("UserTypeId = @userTypeId", new { userTypeId })
    .Where("Role = @role", new { role });

var template = sqlBuilder.AddTemplate("SELECT * FROM Users /**where**/");

using var connection = new SqliteConnection("Data Source=database.db");
var users = connection.Query<User>(template.RawSql, template.Parameters);
```

---

## [Builder (SimpleSqlBuilder)](#tab/builder)

```csharp
using Dapper.SimpleSqlBuilder;

var builder = SimpleBuilder.Create($@"
SELECT * FROM Users
WHERE UserTypeId = {userTypeId}
AND Role = {role}");

using var connection = new SqliteConnection("Data Source=database.db");
var users = connection.Query<User>(builder.Sql, builder.Parameters);
```

---

## [Fluent Builder (SimpleSqlBuilder)](#tab/fluent-builder)

```csharp
using Dapper.SimpleSqlBuilder;

var fluentBuilder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"Users")
    .Where($"UserTypeId = {userTypeId}")
    .Where($"Role = {role}");

using var connection = new SqliteConnection("Data Source=database.db");
var users = connection.Query<User>(fluentBuilder.Sql, fluentBuilder.Parameters);
```

---

## Next Steps

- [Performance](../miscellaneous/performance.md)
