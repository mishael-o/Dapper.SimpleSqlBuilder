# Comparison to Dapper

The code snippets below show how the library compares to Dapper and Dapper's SqlBuilder when writing SQL queries. For performance comparisons, see the [Performance](../miscellaneous/performance.md) section.

The code is extracting data from a `Users` table where the `UserTypeId` is equal to a given `userTypeId` and the `Role` is equal to a given `role`.

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
