# Dapper Simple SQL Builder

[![Continuous integration and delivery](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml) [![Codecov](https://img.shields.io/codecov/c/gh/mishael-o/Dapper.SimpleSqlBuilder?logo=codecov)](https://codecov.io/gh/mishael-o/Dapper.SimpleSqlBuilder)

![Dapper Simple SQL Builder](https://raw.githubusercontent.com/mishael-o/Dapper.SimpleSqlBuilder/main/images/readme-icon.png)

A simple SQL builder for [Dapper](https://github.com/DapperLib/Dapper) using [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) and fluent API for building safe static and dynamic SQL queries.
This is achieved by leveraging [FormattableString](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) and [interpolated string handlers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/interpolated-string-handler) to capture parameters and produce parameterized SQL.

**The library provides a feature set for building and parametrizing SQL queries, however all of Dapper's features and quirks still apply for query parameters.**

## Packages

[Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder): A simple SQL builder for Dapper using string interpolation and fluent API for building safe static and dynamic SQL queries.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder)

[Dapper.SimpleSqlBuilder.StrongName](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName): A package that uses Dapper.StrongName.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName)

[Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection): Dependency injection extension for Dapper.SimpleSqlBuilder.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection)

## Quick Start

Pick the NuGet package that best suits your needs and follow the instructions below.

### Installation

The example below shows how to install the [Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) package.

Install via the NuGet Package Manager Console

```powershell
Install-Package Dapper.SimpleSqlBuilder
```

Or via the .NET Core command line interface

```bash
dotnet add package Dapper.SimpleSqlBuilder
```

### Usage

The library provides two builders for building SQL queries, which can be created via the static `SimpleBuilder` class.

- `Builder` - for building static, dynamic, and complex SQL queries.
- `Fluent Builder` - for building SQL queries using fluent API.

 The library also provides an alternative to static classes via [dependency injection](#dependency-injection).

#### Create SQL query with the `Builder`

```c#
using Dapper.SimpleSqlBuilder;

int userTypeId = 4;
string role = "Admin";

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND Role = {role}");
```

**The concern you might have here is the issue of SQL injection, however this is mitigated by the library as the SQL statement is converted to this.**

```sql
SELECT * FROM User
WHERE Id = @p0 AND Role = @p1
```

**And all values passed into the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's [DynamicParameters](https://github.com/DapperLib/Dapper/blob/main/Dapper/DynamicParameters.cs) collection.**

To execute the query with Dapper is as simple as this:

```c#
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

To learn more about the builder, see the [Builder](#builder) section.

#### Create SQL query with the `Fluent Builder`

```c#
using Dapper.SimpleSqlBuilder;

int userTypeId = 4;
var roles = new[] { "Admin", "User" };

var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}")
    .Where($"Role IN {roles}");

// Execute the query with Dapper
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE UserTypeId = @p0 AND Role IN @p1
```

To learn more about the fluent builder, see the [Fluent Builder](#fluent-builder) section.

#### Simple Builder Settings

To learn about configuring the simple builder, see the [Configuring Simple Builder Settings](#configuring-simple-builder-settings) section.

## Builder

A builder for building static, dynamic, and complex SQL queries.

### Static SQL

```c#
using Dapper.SimpleSqlBuilder;

int userTypeId = 10;
int age = 25;

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND AGE >= {age}");
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND AGE >= @p1
```

For newer versions of C# you can also use [raw string literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string) with [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) to build your SQL queries, instead of [verbatim string literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim). See the example below.

```c#
var builder = SimpleBuilder.Create($"""
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND AGE >= {age}
""");
```

### Dynamic SQL

#### Interpolated String Concatenation

You can concatenate multiple interpolated strings to build your dynamic SQL.

```c#
var users = GetUsers(4, null, 25);

IEnumerable<User> GetUsers(int userTypeId, string role, int? minAge)
{
    var builder = SimpleBuilder.Create($"SELECT * FROM User");
    builder += $" WHERE UserTypeId = {userTypeId}";

    if (role is not null)
    {
        builder += $" AND Role = {role}";
    }

    if (minAge.HasValue)
    {
        builder += $" AND Age >= {minAge}";
    }

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT * FROM User WHERE UserTypeId = @p0 AND Age >= @p1
```

#### Builder Chaining

If you prefer an alternative to interpolated string concatenation, you can use the `Append(...)`, `AppendIntact(...)` and `AppendNewLine(...)` methods, which can be chained.

```c#
int userTypeId = 4;
string role = "User";

var builder = SimpleBuilder.Create($"SELECT * FROM User")
    .AppendNewLine($"WHERE UserTypeId = {userTypeId}")
    .Append($"OR Role = {role}")
    .AppendNewLine($"ORDER BY FirstName ASC");
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 OR Role = @p1
ORDER BY FirstName ASC
```

You can also use it with conditional statements. The `Append(...)`, `AppendIntact(...)` and `AppendNewLine(...)` methods all have conditional overloads. This is useful when you want to append a statement only if a condition is met.

```c#
var users = GetUsers(4, "Admin", null, true);

IEnumerable<User> GetUsers(int userTypeId, string role, int? minAge, bool orderByName)
{
    var builder = SimpleBuilder.Create()
        .AppendIntact($"SELECT * FROM User")
        .AppendNewLine($"WHERE UserTypeId = {userTypeId}")
         .Append(role is not null, $"AND Role = {role}")
        .Append(minAge.HasValue, $"AND Age >= {minAge}")
        .AppendNewLine(orderByName, $"ORDER BY FirstName, LastName");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND Role = @p1
ORDER BY FirstName, LastName
```

**Note**: The `Append(...)` method adds a space before the SQL text by default. You can use the `AppendIntact(...)` method if you don't want this behaviour.

### INSERT, UPDATE and DELETE Statements

#### Insert

You can perform INSERT operations with the builder as seen in the example below.

```c#
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($@"
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES ({user.FirstName}, {user.LastName}, {user.UserTypeId})");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES (@p0, @p1, @p2)
```

#### Update

You can perform UPDATE operations with the builder as seen in the example below.

```c#
int id = 1;
string role = "Admin";

var builder = SimpleBuilder.Create($@"
UPDATE User 
SET Role = {role}
WHERE Id = {id}");
```

The generated SQL will be:

```sql
UPDATE User
SET Role = @p0
WHERE Id = @p1
```

#### Delete

You can perform DELETE operations with the builder as seen in the example below.

```c#
int id = 1;
var builder = SimpleBuilder.Create($"DELETE FROM User WHERE Id = {id}");
```

The generated SQL will be:

```sql
DELETE FROM User WHERE Id = @p0
```

### Stored Procedures

You can execute stored procedures with the builder as seen in the example below.

```c#
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($"CreateUserProc")
    .AddParameter("FirstName", user.FirstName)
    .AddParameter("LastName", user.LastName)
    .AddParameter("UserTypeId", user.UserTypeId)
    .AddParameter("Id", dbType: DbType.Int32, direction: ParameterDirection.Output)
    .AddParameter("Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

// Execute the stored procedure with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

// Get the output and return values
int id = builder.GetValue<int>("Id");
int result = builder.GetValue<int>("Result");
```

## Fluent Builder

A builder for building SQL queries using fluent API.

### Select Builder

You can perform SELECT operations with the fluent builder as seen in the examples below.

#### Select

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Role")
    .From($"User");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName").Select($"LastName").Select($"Role")
    .From($"User");

// Execute the query with Dapper
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Role
FROM User
```

**Another example:**

```c#
string role = "Admin%";
int userTypeId = 10;

var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Role LIKE {role}")
    .OrWhere($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE Role LIKE @p0 OR UserTypeId = @p1
```

For complex `WHERE` clause statements refer to the [Where Filters](#where-filters-complex-filter-statements) section.

#### Select Distinct

```c#
var builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"UserTypeId, Role")
    .From($"User");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"UserTypeId").SelectDistinct($"Role")
    .From($"User");
```

The generated SQL will be:

```sql
SELECT DISTINCT UserTypeId, Role
FROM User
```

#### Join

```c#
int minAge = 18;

var builder = SimpleBuilder.CreateFluent()
    .Select($"u.FirstName, u.LastName, u.Age, ut.Type, us.Status, ua.AddressLine1, ua.AddressLine2")
    .From($"User u")
    .InnerJoin($"UserType ut ON u.UserTypeId = ut.Id")
    .RightJoin($"UserStatus us ON u.UserStatusId = us.Id")
    .LeftJoin($"UserAddress ua ON u.UserAddressId = ua.Id")
    .Where($"u.Age >= {minAge}");
```

The generated SQL will be:

```sql
SELECT u.FirstName, u.LastName, u.Age, ut.Type, us.Status, ua.AddressLine1, ua.AddressLine2
FROM User u
INNER JOIN UserType ut ON u.UserTypeId = ut.Id
RIGHT JOIN UserStatus us ON u.UserStatusId = us.Id
LEFT JOIN UserAddress ua ON u.UserAddressId = ua.Id
WHERE u.Age >= @p0
```

#### Group By

```c#
var roles = new[] { "Admin", "User" };

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, UserTypeId, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role NOT IN {roles}")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role, UserTypeId");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"Role, UserTypeId, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role NOT IN {roles}")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role").GroupBy($"UserTypeId");
```

The generated SQL will be:

```sql
SELECT Role, UserTypeId, COUNT(Id) AS UserCount
FROM User
WHERE Role NOT IN @p0 AND Role IS NOT NULL
GROUP BY Role, UserTypeId
```

#### Having

```c#
int minAge = 18;

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, Age, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role, Age")
    .Having($"COUNT(Id) > 1").Having($"Age >= {minAge}");
```

The generated SQL will be:

```sql
SELECT Role, Age COUNT(Id) AS UserCount
FROM User
WHERE Role IS NOT NULL
GROUP BY Role, Age
HAVING COUNT(Id) > 1 AND Age >= @p0
```

#### Order By

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName")
    .From($"User")
    .OrderBy($"FirstName ASC, LastName DESC");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName")
    .From($"User")
    .OrderBy($"FirstName ASC").OrderBy($"LastName DESC");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName
FROM User
ORDER BY FirstName ASC, LastName DESC
```

#### Pagination

The `SELECT` builder supports two popular ways of performing pagination. **You should use the methods that are supported by your database.**

**`Limit` and `Offset` methods:**

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age")
    .From($"User")
    .OrderBy($"Age ASC")
    .Limit(10)
    .Offset(20);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age
FROM User
ORDER BY Age ASC
LIMIT 10 OFFSET 20
```

**`OffsetRows` and `FetchNext` methods:**

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age")
    .From($"User")
    .OrderBy($"Age ASC")
    .OffsetRows(20)
    .FetchNext(10);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age
FROM User
ORDER BY Age ASC
OFFSET 20 ROWS 
FETCH NEXT 10 ROWS ONLY
```

### Insert Builder

You can perform INSERT operations with the fluent builder as seen in the example below.

```c#
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.CreateFluent()
    .InsertInto($"User")
    .Columns($"FirstName, LastName, UserTypeId")
    .Values($"{user.FirstName}, {user.LastName}, {user.UserTypeId}");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
   .InsertInto($"User")
   .Columns($"FirstName").Columns($"LastName").Columns($"UserTypeId")
   .Values($"{user.FirstName}").Values($"{user.LastName}").Values($"{user.UserTypeId}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES (@p0, @p1, @p2)
```

### Update Builder

You can perform UPDATE operations with the fluent builder as seen in the example below.

```c#
int id = 1;
int userTypeId = 4;
string role = "User";

var builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"UserTypeId = {userTypeId}, Role = {role}")
    .Where($"Id = {id}");

// The query can also be written as below
builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"UserTypeId = {userTypeId}")
    .Set($"Role = {role}")
    .Where($"Id = {id}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
UPDATE User
SET UserTypeId = @p0, Role = @p1
WHERE Id = @p2
```

For complex `WHERE` clause statements refer to the [Where Filters](#where-filters-complex-filter-statements) section.

### Delete Builder

You can perform DELETE operations with the fluent builder as seen in the example below.

```c#
int id = 10;

var builder = SimpleBuilder.CreateFluent()
    .DeleteFrom($"User")
    .Where($"Id = {id}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
DELETE FROM User
WHERE Id = @p0
```

### Where Filters (Complex filter statements)

The fluent builder supports complex filters, which means you can add `WHERE`, `AND`, and `OR` clauses with complex filter statements.

#### WhereFilter

The `WhereFilter` method adds a `WHERE` filter statement enclosed in parenthesis to the query. Subsequent `WhereFilter` method calls adds an `AND` filter statement to the query.

The `WhereFilter` method can be combined with the `WithFilter` and `WithOrFilter` methods to add `AND` and `OR` filters respectively within the filter statement.

```c#
int minAge = 20;
int maxAge = 50;
int userTypeId = 4;

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .Where($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Age >= @p0 AND Age < @p1) AND UserTypeId = @p2
```

**Another example:**

```c#
int minAge = 20;
int maxAge = 50;
string adminRole = "Admin";
string userRole = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .WhereFilter($"Role = {adminRole}").WithOrFilter($"Role = {userRole}").WithOrFilter($"Role IS NULL"); 
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Age >= @p0 AND Age < @p1) AND (Role = @p2 OR Role = @p3 OR Role IS NULL)
```

#### OrWhereFilter

The `OrWhereFilter` method adds an `OR` filter statement enclosed in parenthesis to the query.

The `OrWhereFilter` method can be combined with the `WithFilter` and `WithOrFilter` methods to add `AND` and `OR` filters respectively within the filter statement.

```c#
int userTypeId = 4;
int minAge = 30;
int maxAge = 65;
string role = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhereFilter($"Role = {role}").WithOrFilter($"Role IS NULL");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE UserTypeId = @p0 OR (Age >= @p1 AND Age < @p2) OR (Role = @p3 OR Role IS NULL)
```

**Another example:**

```c#
int minAge = 20;
int maxAge = 50;
var roles = new [] { "Admin", "User" };
string userRole = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Role IN {roles}").WithOrFilter($"Role IS NULL")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhere($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Role IN @p0 OR Role IS NULL) OR (Age >= @p1 AND Age < @p2) OR UserTypeId = @p3
```

### Conditional Methods (Clauses)

The fluent builder supports conditional methods (clauses). This is useful when you want to add a clause only if a condition is met.

The `Set(...)`, `InnerJoin(...)`, `RightJoin(...)`, `LeftJoin(...)`, `Where(...)`, `OrWhere(...)`, `WithFilter(...)`, `WithOrFilter(...)`, `GroupBy(...)`, `Having(...)` and `OrderBy(...)` methods all have conditional overloads.

#### Conditional methods: Example 1

```c#
var users = GetUsers("John", null, "Admin", true);

IEnumerable<User> GetUsers(string firstNameSearch, int? userTypeId, string role, bool orderByFirstName)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .Where(!string.IsNullOrWhiteSpace(firstNameSearch), $"FirstName LIKE {'%' + firstNameSearch + '%'}")
        .Where(userTypeId.HasValue, $"UserTypeId = {userTypeId}")
        .Where(role != null, $"Role = {role}")
        .OrderBy(orderByFirstName, $"FirstName ASC");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE FirstName LIKE @p0 AND Role = @p1
ORDER BY FirstName ASC
```

#### Conditional methods: Example 2

```c#
var filter = new Filter { UserTypeId = null, Roles = new [] { "Admin", "User" }, IncludeUsersWithoutRole = true };
var users = GetUsers(filter);

IEnumerable<User> GetUsers(Filter filter)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .Where(filter?.UserTypeId.HasValue == true, $"UserTypeId = {filter.UserTypeId}")
        .OrWhere(filter?.Roles?.Length > 0, $"Role IN {filter.Roles}")
        .OrWhere(filter?.IncludeUsersWithoutRole == true, $"Role IS NULL");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE Role IN @p0 OR Role IS NULL
```

#### Conditional methods: Example 3

```c#
var result = UpdateUser(10, "John", "Doe", null, 30);

bool UpdateUser(int id, string firstName, string lastName, string role, int? userTypeId)
{
    var builder = SimpleBuilder.CreateFluent()
        .Update($"User")   
        .Set($"FirstName = {firstName}")
        .Set($"LastName = {lastName}")
        .Set(role is not null, $"Role = {role}")
        .Set(userTypeId.HasValue, $"UserTypeId = {userTypeId}")
        .Where($"Id = {id}");

    return dbConnection.Execute(builder.Sql, builder.Parameters) > 0;
}
```

The generated SQL will be:

```sql
UPDATE User
SET FirstName = @p0, LastName = @p1, UserTypeId = @p2
WHERE Id = @p3
```

#### Conditional methods: Example 4

```c#
var roles = new[] { "Admin", "User" };
var userTypeIds = new[] { 1, 2, 3 };

var users = GetUsers(roles, false, userTypeIds, true);

IEnumerable<User> GetUsers(string[] roles, bool includeUsersWithoutRole, int[] userTypeIds, bool includeUsersWithoutUserTypeId)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .WhereFilter()
            .WithFilter(roles?.Length > 0, $"Role IN {roles}")
            .WithFilter(includeUsersWithoutRole, $"Role IS NULL")
        .OrWhereFilter()
            .WithFilter(userTypeIds?.Length > 0, $"UserTypeId IN {userTypeIds}")
            .WithOrFilter(includeUsersWithoutUserTypeId, $"UserTypeId IS NULL");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE (Role IN @p0) OR (UserTypeId IN @p1)
```

### Lower Case Clauses

The fluent builder supports using lower case clauses. This is applicable to the `Delete`, `Insert`, `Update` and `Select` fluent builders.

The example below shows how to use lower case clauses.

```c#
// Configuring globally. Can also be configured per fluent builder instance.
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);

int userTypeId = 1;
int minAge = 20;
int maxAge = 50;

var builder = SimpleBuilder.CreateFluent()
    .Select($"u.Role, u.Age, ut.Type, COUNT(u.Id) AS UserCount")
    .From($"User u")
    .InnerJoin($"UserType ut ON u.UserTypeId = ut.Id")
    .Where($"u.Role IN {roles}")
    .OrWhere($"u.UserTypeId = {userTypeId}")
    .GroupBy($"u.Role, u.Age, ut.Type")
    .Having($"u.Age >= {minAge}").Having($"u.Age < {maxAge}")
    .OrderBy($"u.Role ASC");
```

The generated SQL will be:

```sql
select u.Role, u.Age, ut.Type, COUNT(u.Id) AS UserCount
from User u
inner join UserType ut ON u.UserTypeId = ut.Id
where u.Role IN @p0 or u.UserTypeId = @p1
group by u.Role, u.Age, ut.Type
having u.Age >= @p2 and u.Age < @p3
order by u.Role ASC
```

## Formattable Strings

The library supports passing [formattable strings](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) to the builders. This is useful for scenarios such as subqueries, and breaking complex queries into smaller ones.

```c#
int userTypeId = 10;
FormattableString subQuery = $"SELECT Type from UserType WHERE Id = {userTypeId}";

var builder = SimpleBuilder.Create($@"
SELECT x.*, ({subQuery}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type from UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p1
```

## Parameter Properties

The library enables you to configure parameter properties via the `AddParameter(...)` method. For example, you may want to define a `DbType` for a parameter, and the code below is how you will do this.

```c#
int id = 10;

var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = @id")
    .AddParameter("id", value: id, dbType: DbType.Int32);
```

However, the library also provides an extension method to easily achieve this while using interpolated strings.

```c#
using Dapper.SimpleSqlBuilder.Extensions;

// Define parameter properties
var idParam = id.DefineParam(dbType: DbType.Int32);
var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = {idParam}");

// OR

// Defining parameter properties in-line
var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Id = {id.DefineParam(dbType: DbType.Int32)}");
```

The `DefineParam(...)` extension method enables you to define the `DbType`, `Size`, `Precision` and `Scale` of your parameter. This should only be used for parameters passed into the interpolated string, as the parameter direction is always set to `Input` for values in the interpolated string.

As an alternative to the extension method you can manually create the parameter object.

```c#
var idParam = new SimpleParameterInfo(id, dbType: DbType.Int64);
```

## Configuring Simple Builder Settings

You can configure the simple builder settings through the `SimpleBuilderSettings` static class by calling the `Configure(...)` method. **However, if you are using the dependency injection library refer to the [Dependency Injection](#dependency-injection) section on how to configure the global simple builder settings**.

The code below shows how to do this.

```c#
SimpleBuilderSettings.Configure
(
    parameterNameTemplate: "param", // Optional. Default is "p"
    parameterPrefix: ":", // Optional. Default is "@"
    reuseParameters: true // Optional. Default is "false"
    useLowerCaseClauses: true // Optional. Default is "false". This is only applicable to the fluent builder.
);
```

### Configuring Parameter Name Template

The default parameter name template is `p`, meaning when parameters are created they will be named `p0 p1 p2 ...` You can configure this by passing your desired value to the `parameterNameTemplate` parameter.

```c#
SimpleBuilderSettings.Configure(parameterNameTemplate: "param");
```

### Configuring Parameter Prefix

The default parameter prefix is `@`, meaning when parameters are passed to the database they will be passed as `@p0 @p1 @p2 ...`, however this may not be applicable to all databases. You can configure this by passing your desired value to the `parameterPrefix` parameter.

```c#
SimpleBuilderSettings.Configure(parameterPrefix: ":");
```

This can also be configured per simple builder instance if you want to override the global settings.

```c#
// Builder
var builder = SimpleBuilder.Create(parameterPrefix: ":");

// Fluent builder
var fluentBuilder = SimpleBuilder.CreateFluent(parameterPrefix: ":");
```

### Configuring Parameter Reuse

The library supports parameter reuse, and the default is `false`. Go to the [Reusing Parameters](#reusing-parameters) section to learn more. You can configure this by passing your desired value to the `reuseParameters` parameter.

```c#
SimpleBuilderSettings.Configure(reuseParameters: true);
```

This can also be configured per simple builder instance if you want to override the global settings.

```c#
// Builder
var builder = SimpleBuilder.Create(reuseParameters: true);

// Fluent builder
var fluentBuilder = SimpleBuilder.CreateFluent(reuseParameters: true);
```

### Configuring Fluent builder to use Lower Case Clauses

The library supports using lower case clauses for the **fluent builder**, and the default is `false`. You can configure this by passing your desired value to the `useLowerCaseClauses` parameter.

```c#
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);
```

This can also be configured per fluent builder instance if you want to override the global settings.

```c#
var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true);
```

## Reusing Parameters

The library supports reusing the same parameter name for parameters with the same value, type, and properties. This is turned off by default, however can be enabled globally via the simple builder settings or per simple builder instance.

**Note:** Parameter reuse does not apply to `null` values.

The example below shows how.

```c#
// Configuring globally. Can also be configured per simple builder instance.
SimpleBuilderSettings.Configure(reuseParameters: true);

int maxAge = 30;
int userTypeId = 10;

var builder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type from UserType WHERE Id = {userTypeId}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}");
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type from UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1
```

## Raw values (:raw)

There are scenarios where you may want to pass a raw value into the interpolated string and not parameterize the value. The `raw` format string is used to indicate that the value should not be parameterized.

**Note: Do not use raw values if you don't trust the source or have not sanitized your value, as this can lead to SQL injection.**

### Column and Table names with `nameof()`

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"{nameof(User.Id):raw}, {nameof(User.FirstName):raw}, {nameof(User.LastName):raw}")
    .From($"{nameof(User):raw}");
```

The generated SQL will be:

```sql
SELECT Id, FirstName, LastName
FROM User
```

### Named Parameter

This example uses T-SQL (MSSQL) syntax, however the same applies to other databases.

```c#
const string idParamName = "id";
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($@"
BEGIN
    SELECT @{idParamName:raw} = NEXT VALUE FOR UserSequence;
    INSERT INTO User (Id, FirstName, LastName, UserTypeId)
    VALUES (@{idParamName:raw}, {user.FirstName}, {user.LastName}, {user.UserTypeId});
END")
.AddParameter(idParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

dbConnection.Execute(builder.Sql, builder.Parameters);
var id = builder.GetValue<int>(idParamName);
```

The generated SQL will be:

```sql
BEGIN
    SELECT @id = NEXT VALUE FOR UserSequence;
    INSERT INTO User (Id, FirstName, LastName, UserTypeId)
    VALUES (@id, @p0, @p1, @p2);
END
```

## Dependency Injection

An alternative to using the static classes to access the simple builder and settings is via dependency injection. Use the [Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) nuget package instead of the default package. The library supports the default dependency injection pattern in .Net Core.

```c#
using Dapper.SimpleSqlBuilder.DependencyInjection;

services.AddSimpleSqlBuilder();
```

Usage in a class.

```c#
class MyClass
{
    private readonly simpleBuilder;

    public MyClass(ISimpleBuilder simpleBuilder)
    {
        this.simpleBuilder = simpleBuilder;
    }

    public void MyMethod()
    {
        int id = 10;
        var builder = simpleBuilder.Create($"SELECT * FROM User WHERE Id = {id}");

        // Other code below .....
    }

    public void MyMethod2()
    {
        int id = 10;
        var builder = simpleBuilder.CreateFluent()
            .Select($"*")
            .From($"User")
            .Where($"Id = {id}");

        // Other code below .....
    }
}
```

### Configuring Simple Builder Options

You can configure the simple builder settings and the `ISimpleBuilder` instance service lifetime. The various methods are described below.

#### Configuring Simple Builder Settings via `appsettings.json`

```json
{
  "SimpleSqlBuilder": {
    "DatabaseParameterNameTemplate": "p",
    "DatabaseParameterPrefix": "@",
    "ReuseParameters": false,
    "UseLowerCaseClauses": false
  }
}
```

```c#
services.AddSimpleSqlBuilder(
    // Optional. Default is ServiceLifetime.Singleton
    serviceLifeTime = ServiceLifetime.Singleton);
```

#### Configuring Simple Builder Settings via code

```c#
services.AddSimpleSqlBuilder(
    configure =>
    {
        configure.DatabaseParameterNameTemplate = "param"; // Optional. Default is "p"
        configure.DatabaseParameterPrefix = ":"; // Optional. Default is "@"
        configure.ReuseParameters = true; // Optional. Default is "false"
        configure.UseLowerCaseClauses = true; // Optional. Default is "false". This is only applicable to the fluent builder
    },
    // Optional. Default is ServiceLifetime.Singleton
    serviceLifeTime = ServiceLifetime.Scoped);
```

## Database Support

The library supports any database that Dapper supports. However, the library has been tested against the latest versions of MSSQL, MySQL and PostgreSQL databases. The integration test can be found here [SimpleSqlBuilder.IntegrationTests](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Tests/IntegrationTests/SimpleSqlBuilder.IntegrationTests). They provide a good example of how to use the library.

## Benchmark

The benchmark below shows the performance of the `Builder` and `Fluent Builder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1778)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.302
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-UDVULW : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-ZBHUIE : .NET Framework 4.8.1 (4.8.9139.0), X64 RyuJIT VectorSize=256

```

|                             Method |              Runtime |   Categories |      Mean | Allocated |
|----------------------------------- |--------------------- |------------- |----------:|----------:|
|                SqlBuilder (Dapper) |             .NET 7.0 | Simple query |  1.865 μs |   2.92 KB |
|                            Builder |             .NET 7.0 | Simple query |  1.531 μs |   4.43 KB |
|                      FluentBuilder |             .NET 7.0 | Simple query |  2.001 μs |    4.5 KB |
|         Builder (Reuse parameters) |             .NET 7.0 | Simple query |  2.195 μs |    4.7 KB |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 | Simple query |  2.755 μs |   4.77 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.237 μs |   3.43 KB |
|                            Builder | .NET Framework 4.6.1 | Simple query |  3.821 μs |    4.7 KB |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.493 μs |    5.2 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  4.607 μs |   5.27 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.260 μs |   5.77 KB |
|                                    |                      |              |           |           |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) |             .NET 7.0 |  Large query | 28.193 μs |  42.19 KB |
|                            Builder |             .NET 7.0 |  Large query | 21.475 μs |  48.79 KB |
|                      FluentBuilder |             .NET 7.0 |  Large query | 26.700 μs |  48.62 KB |
|         Builder (Reuse parameters) |             .NET 7.0 |  Large query | 14.929 μs |  29.34 KB |
|   FluentBuilder (Reuse parameters) |             .NET 7.0 |  Large query | 20.039 μs |  29.18 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 43.275 μs |   53.1 KB |
|                            Builder | .NET Framework 4.6.1 |  Large query | 52.571 μs |  62.15 KB |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 63.775 μs |  68.61 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 39.589 μs |  37.42 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 50.712 μs |  43.87 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.

## Contributing

Refer to the [Contributing](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CONTRIBUTING.md) guide for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/LICENSE.md) file for details.

## Acknowledgements

- Thanks to [JetBrains](https://www.jetbrains.com) for their open source development [support](https://jb.gg/OpenSourceSupport).
- This project was inspired by these amazing libraries. [Dapper SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) and [DapperQueryBuilder](https://github.com/Drizin/DapperQueryBuilder).
