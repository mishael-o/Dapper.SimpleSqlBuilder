# Dapper Simple SQL Builder

[![Continuous integration and delivery](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml) [![Codecov](https://img.shields.io/codecov/c/gh/mishael-o/Dapper.SimpleSqlBuilder?logo=codecov)](https://codecov.io/gh/mishael-o/Dapper.SimpleSqlBuilder)

A simple SQL builder for [Dapper](https://github.com/DapperLib/Dapper) using string interpolation and fluent API for building dynamic SQL.

This library provides a simple and easy way to build dynamic SQL and commands, that can be executed using the Dapper library. This is achieved by leveraging [FormattableString](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) and [interpolated string handlers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/interpolated-string-handler) to capture parameters and produce parameterized SQL.

**The library provides a feature set for building and parametrizing SQL queries, however all of Dapper's features and quirks still apply for query parameters.**

## Packages

[Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder): A simple SQL builder for Dapper using string interpolation and fluent API.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder)

[Dapper.SimpleSqlBuilder.StrongName](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName): A package that uses Dapper.StrongName.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName)

[Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection): Dependency injection extension for Dapper.SimpleSqlBuilder.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection)

## Quick Start

Pick the Nuget package that best suits your needs and follow the instructions below.

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

The library provides a static class called `SimpleBuilder` that is used to create simple builder instances. However, the library also provides an alternative to the static classes via dependency injection, which is covered in the [Dependency Injection](#dependency-injection) section.

#### Create SQL query with the Builder

```c#
using Dapper.SimpleSqlBuilder;

string name = "John";

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE Name = {name}");
```

**The concern you might have here is the issue of SQL injection, however this is mitigated by the library as the SQL statement is converted to this.**

```sql
SELECT * FROM User
WHERE Name = @p0
```

**And all values passed into the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's [DynamicParameters](https://github.com/DapperLib/Dapper/blob/main/Dapper/DynamicParameters.cs) collection.**

To execute the query with Dapper is as simple as this.

```c#
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

To learn more about the builder, see the [Builder](#builder) section.

#### Create SQL query with the Fluent Builder

```c#
using Dapper.SimpleSqlBuilder;

var roles = new[] { "Admin", "User" };
int age = 25;

var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Role IN {roles}")
    .Where($"Age >= {age}");

// The generated SQL will be.
// SELECT *
// FROM User
// WHERE Role IN @p0 AND Age >= @p1

// Execute the query with Dapper
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

To learn more about the fluent builder, see the [Fluent Builder](#fluent-builder) section.

#### Simple Builder Settings

To learn about configuring the simple builder, see the [Configuring Simple Builder Settings](#configuring-simple-builder-settings) section.

## Builder

### Static SQL

```c#
using Dapper.SimpleSqlBuilder;

int userTypeId = 10;
int age = 25;

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND AGE >= {age}");
```

The generated SQL will be.

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND AGE >= @p1
```

### Dynamic SQL

You can concatenate multiple interpolated strings to build your dynamic SQL.

```c#
var user = new User { TypeId = 10, Role = "Admin", Age = 20 };

var builder = SimpleBuilder.Create($"SELECT * FROM User");
builder += $" WHERE UserTypeId = {user.TypeId}";

if (user.Age is not null)
{
    builder += $" AND AGE >= {user.Age}"
}
```

The generated SQL will be.

```sql
SELECT * FROM User WHERE UserTypeId = @p0 AND AGE >= @p1
```

### Builder Chaining

If you prefer an alternative to interpolated string concatenation, you can use the `Append(...)`, `AppendIntact(...)` and `AppendNewLine(...)` methods, which can be chained.

```c#
var builder = SimpleBuilder.Create($"SELECT * FROM User")
    .AppendNewLine($"WHERE UserTypeId = {id}")
    .Append($"AND Age >= {age}")
    .AppendNewLine($"ORDER BY Age ASC");
```

The generated SQL will be.

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND Age >= @p1
ORDER BY Name ASC
```

You can also use it with conditional statements.

```c#
var builder = SimpleBuilder.Create()
    .Append($"SELECT * FROM User WHERE UserTypeId = {user.TypeId}");

if (user.Age is not null)
{
    builder.Append($"AND Age >= {user.Age}");
}

builder.Append($"ORDER BY Age ASC");
```

**Note**: The `Append(...)` method adds a space before the SQL text by default. You can use the `AppendIntact(...)` method if you don't want this behaviour.

### INSERT, UPDATE and DELETE Statements

#### INSERT Statement

You can perform INSERT operations with the builder as seen in the example below.

```c#
var builder = SimpleBuilder.Create($@"
INSERT INTO User (Role, Age)
VALUES ({user.Role}, {user.Age}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be.

```sql
INSERT INTO User (Role, Age)
VALUES (@p0, @p1)
```

#### UPDATE Statement

You can perform UPDATE operations with the builder as seen in the example below.

```c#
var builder = SimpleBuilder.Create($@"
UPDATE User SET Role = {user.Role}
WHERE Id = {user.Id}");
```

The generated SQL will be.

```sql
UPDATE User
SET Role = @p0
WHERE Id = @p1
```

#### DELETE Statement

You can perform DELETE operations with the builder as seen in the example below.

```c#
var builder = SimpleBuilder.Create($"DELETE FROM User WHERE Id = {user.Id}");
```

The generated SQL will be.

```sql
DELETE FROM User WHERE Id = @p0
```

### Stored Procedures

You can execute stored procedures with the builder as seen in the example below.

```c#
var builder = SimpleBuilder.Create($"UserResources.ProcessUserInformation")
    .AddParameter("UserRole", userRole)
    .AddParameter("UserAge", userAge)
    .AddParameter("UserId", dbType: DbType.Int32, direction: ParameterDirection.Output)
    .AddParameter("Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

// Execute the stored procedure with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

// Get the output and return values
int id = builder.GetValue<int>("UserId");
int result = builder.GetValue<int>("Result");
```

## Fluent Builder

The fluent builder provides a simple and fluent way of building SQL queries.

### SELECT Builder

You can perform SELECT operations with the fluent builder as seen in the examples below.

#### Example 1: SELECT

```c#
int age = 20;
int userTypeId = 10;

var builder = SimpleBuilder.CreateFluent()
    .Select($"Name, Age, Role")
    .From($"User")
    .Where($"Age > {age}")
    .Where($"UserTypeId = {userTypeId}")
    .OrderBy($"Name ASC, Age DESC");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"Name").Select($"Age").Select($"Role")
    .From($"User")
    .Where($"Age > {age}")
    .Where($"UserTypeId = {userTypeId}")
    .OrderBy($"Name ASC").OrderBy($"Age DESC");


// Execute the query with Dapper
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

The generated SQL will be.

```sql
SELECT Name, Age, Role
FROM User
WHERE Age > @p0 AND UserTypeId = @p1
ORDER BY Name ASC, Age DESC
```

#### Example 2: SELECT DISTINCT

```c#
var builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"Name, Age, Role")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"Name").SelectDistinct($"Age").SelectDistinct($"Role")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}");
```

The generated SQL will be.

```sql
SELECT DISTINCT Name, Age, Role
FROM User
WHERE UserTypeId = @p0
```

#### Example 3: SELECT

```c#
int age = 20;
string role = "Admin";

var builder = SimpleBuilder.CreateFluent()
    .Select($"Name, Age, Role")
    .From($"User u")
    .InnerJoin($"UserType ut ON u.UserTypeId = ut.Id")
    .RightJoin($"UserStatus us ON u.UserStatusId = us.Id")
    .LeftJoin($"UserAddress ua ON u.UserAddressId = ua.Id")
    .Where($"Age > {age}")
    .OrWhere($"Role = {role}")
    .OrderBy($"Age DESC");
```

The generated SQL will be.

```sql
SELECT Name, Age, Role
FROM User u
INNER JOIN UserType ut ON u.UserTypeId = ut.Id
RIGHT JOIN UserStatus us ON u.UserStatusId = us.Id
LEFT JOIN UserAddress ua ON u.UserAddressId = ua.Id
WHERE Age > @p0 OR Role = @p1
ORDER BY Age DESC
```

#### Example 4: SELECT

```c#
var roles = new[] { "Admin", "User" };
int minAge = 20;
int maxAge = 30;

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, Name, COUNT(Age) AS AgeCount")
    .From($"User")
    .Where($"Role IN {roles}")
    .GroupBy($"Role, Name")
    .Having($"Age >= {minAge}").Having($"Age < {maxAge}")
    .OrderBy($"Role ASC");
```

The generated SQL will be.

```sql
SELECT Role, Name, COUNT(Age) AS AgeCount
FROM User
WHERE Role IN @p0
GROUP BY Role, Name
HAVING Age >= @p1 AND Age < @p2
ORDER BY Role ASC
```

### INSERT Builder

You can perform INSERT operations with the fluent builder as seen in the examples below.

#### Example 1: INSERT

```c#
var user = new User { Id = 10, Name = "John" Role = "Admin", Age = 20 };

var builder = SimpleBuilder.CreateFluent()
    .InsertInto($"User")
    .Columns($"Id, Name, Role, Age")
    .Values($"{user.Id}, {user.Name}, {user.Role}, {user.Admin}");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
   .InsertInto($"User")
   .Columns($"Id").Columns($"Role").Columns($"Age")
   .Values($"{user.Id}").Values($"{user.Name}").Values($"{user.Role}").Values($"{user.Admin}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be.

```sql
INSERT INTO User (Id, Role, Age)
VALUES (@p0, @p1, @p2)
```

#### Example 2: INSERT

```c#
var builder = SimpleBuilder.CreateFluent()
    .InsertInto($"User")
    .Values($"{user.Id}, {user.Name}, {user.Role}, {user.Admin}");

```

The generated SQL will be.

```sql
INSERT INTO User
VALUES (@p0, @p1, @p2)
```

### UPDATE Builder

You can perform UPDATE operations with the fluent builder as seen in the example below.

```c#
var builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"Name = {user.Name}, Role = {user.Role}, Age = {user.Age}")
    .Where($"Id = {user.Id}");

// The query can also be written as below
builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"Name = {user.Name}").Set($"Role = {user.Role}").Set($"Age = {user.Age}")
    .Where($"Id = {user.Id}");
```

The generated SQL will be.

```sql
UPDATE User
SET Name = @p0, Role = @p1, Age = @p2
WHERE Id = @p3
```

### DELETE Builder

You can perform DELETE operations with the fluent builder as seen in the example below.

```c#
int userTypeId = 10;
string role = "Admin";

var builder = SimpleBuilder.CreateFluent()
    .DeleteFrom($"User")
    .Where($"UserTypeId = {id}")
    .Where($"Role = {role}");
```

The generated SQL will be.

```sql
DELETE FROM User
WHERE UserTypeId = @p0 AND Role = @p1
```

### Filters (Complex filter statements)

The fluent builder supports complex filters. This means that you can add WHERE, AND, and OR clauses with complex filter statements.

#### Example 1: Filters

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"Name, Age, Role")
    .From($"User")
    .WhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .Where($"UserTypeId = {userTypeId}")
    .WhereFilter($"Role = {adminRole}").WithOrFilter($"Role = {userRole}").WithOrFilter($"Role IS NULL");
    
```

The generated SQL will be.

```sql
SELECT Name, Age, Role
FROM User
WHERE (Age >= @p0 AND Age < @p1) AND UserTypeId = @p2 AND (Role = @p3 OR Role = @p4 OR Role IS NULL)
```

#### Example 2: Filters

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"Name, Age, Role")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhereFilter($"Role = {adminRole}").WithOrFilter($"Role IS NULL");
```

The generated SQL will be.

```sql
SELECT Name, Age, Role
FROM User
WHERE UserTypeId = @p0 OR (Age >= @p1 AND Age < @p2) OR (Role = @p3 OR Role IS NULL)
```

#### Example 3: Filters

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"Name, Age, Role")
    .From($"User")
    .WhereFilter($"Role = {adminRole}").WithOrFilter($"Role = {userRole}")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhere($"UserTypeId = {userTypeId}");
```

The generated SQL will be.

```sql
SELECT Name, Age, Role
FROM User
WHERE (Role = @p0 OR Role = @p1) OR (Age >= @p2 AND Age < @p3) OR UserTypeId = @p4
```

### Conditional Methods (Clauses)

The fluent builder supports conditional methods (clauses). This is useful when you want to add a clause only if a condition is met. The `Set(...)`, `Where(...)`, `OrWhere(...)`, `WithFilter(...)`, `WithOrFilter(...)`, `GroupBy(...)`, `Having(...)` and `OrderBy(...)` methods all have conditional overloads.

#### Example 1: Conditional Methods

```c#
var user = new User { Id = 10, Name = "John" Role = null, UserTypeId = 123 };

var builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set(user.Name is not null, $"Name = {user.Name}")
    .Set(user.Role is not null, $"Role = {user.Role}")
    .Where($"Id = {user.Id}")
    .Where(user.Role is not null, $"Role = {user.Role}")
    .OrWhere(user.UserTypeId is not null, $"UserTypeId = {user.UserTypeId}");
```

The generated SQL will be.

```sql
UPDATE User
SET Name = @p0
WHERE Id = @p1 OR UserTypeId = @p2
```

#### Example 2: Conditional Methods

```c#
var user = new User { Id = 10, Name = "John" Role = null, Age = null, UserTypeId = 123 };

var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .WhereFilter()
        .WithFilter(user.Role is not null, $"Role = {user.Role}")
        .WithFilter(user.Name is not null, $"Name = {user.Name}")
    .OrWhereFilter()
        .WithFilter(user.Age is not null, $"Age = {user.Age}")
        .WithOrFilter(user.UserTypeId is not null, $"UserTypeId = {user.UserTypeId}");
```

The generated SQL will be.

```sql
SELECT *
FROM User
WHERE (Name = @p0) OR (UserTypeId = @p1)
```

### Lower Case Clauses

The fluent builder supports using lower case clauses. This is applicable to the `Delete`, `Insert`, `Update` and `Select` fluent builders.

The example below shows how to use lower case clauses.

```c#
// Configuring globally. Can also be configured per fluent builder instance.
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, Name, COUNT(Age) AS AgeCount")
    .From($"User")
    .Where($"Role IN {roles}")
    .OrWhere($"Role IS NULL")
    .GroupBy($"Role, Name")
    .Having($"Age >= {minAge}").Having($"Age < {maxAge}")
    .OrderBy($"Role ASC");
```

The generated SQL will be.

```sql
select Role, Name, COUNT(Age) AS AgeCount
from User
where Role IN (@p0) or Role IS NULL
group by Role, Name
having Age >= @p1 and Age < @p2
order by Role ASC
```

## Formattable Strings

The library supports passing [formattable strings](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) within the interpolated string to the builder.

```c#
int userTypeId = 10;
FormattableString subQuery = $"SELECT Description from UserType WHERE Id = {userTypeId}";

var builder = SimpleBuilder.Create($@"
SELECT x.*, ({subQuery}) AS Description
FROM User x
WHERE UserTypeId = {userTypeId}");
```

The generated SQL will be.

```sql
SELECT x.*, (SELECT Description from UserType WHERE Id = @p0) AS Description
FROM User x
WHERE UserTypeId = @p1;
```

## Parameter Properties

The library enables you to configure parameter properties via the `AddParameter(...)` method. For example, you may want to define a `DbType` for a parameter, and the code below is how you will do this.

```c#
var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = @id")
    .AddParameter("id", value: user.Id, dbType: DbType.Int64);
```

However, the library also provides an extension method to easily achieve this while using interpolated strings.

```c#
using Dapper.SimpleSqlBuilder.Extensions;

// Define parameter properties
var idParam = user.Id.DefineParam(dbType: DbType.Int64);
var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = {idParam}");

// OR

// Defining parameter properties inline
var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Id = {user.Id.DefineParam(dbType: DbType.Int64)}");
```

The `DefineParam(...)` extension method enables you to define the `DbType`, `Size`, `Precision` and `Scale` of your parameter. This should only be used for parameters passed into the interpolated string, as the parameter direction is always set to `Input` for values in the interpolated string.

As an alternative to the extension method you can manually create the parameter object.

```c#
var idParam = new SimpleParameterInfo(dbType: DbType.Int64);
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
var builder = SimpleBuilder.CreateFluent(parameterPrefix: ":");
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
var builder = SimpleBuilder.CreateFluent(reuseParameters: true);
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
SELECT x.*, (SELECT Description from UserType WHERE Id = {userTypeId}) AS Description
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}");
```

The generated SQL will be.

```sql
SELECT x.*, (SELECT Description from UserType WHERE Id = @p0) AS Description
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1"
```

## Raw values (:raw)

There are scenarios where you may want to pass a raw value into the interpolated string and not parameterize the value. The `raw` format string is used to indicate that the value should not be parameterized.

**Note: Do not use raw values if you don't trust the source or have not sanitized your value, as this can lead to SQL injection.**

### Example 1: Dynamic Data Retrieval

```c#
string tableName = "User";
DateTime createDate = DateTime.Now;

var builder = SimpleBuilder.Create($"SELECT * FROM {tableName:raw} WHERE CreatedDate = {createDate}");
var tableData = dbConnection.Query(builder.Sql, builder.Parameters);
```

The generated SQL will be.

```sql
SELECT * FROM User WHERE CreatedDate = @p0
```

### Example 2 : Column and table names with nameof()

```c#
var builder = SimpleBuilder.CreateFluent()
    .Select($"{nameof(User.Id):raw}, {nameof(User.Name):raw}, {nameof(User.Age):raw}")
    .From($"{nameof(User):raw}");
```

The generated SQL will be.

```sql
SELECT Id, Name, Age
FROM User
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
        var builder = simpleBuilder.Create($"SELECT * FROM User WHERE ID = {id}");

        // Other code below .....
    }

    public void MyMethod2()
    {
        int id = 10;
        var builder = simpleBuilder.CreateFluent()
            .Select($"*")
            .From($"User")
            .Where($"ID = {id}");

        // Other code below .....
    }
}
```

### Configuring Simple Builder Options

You can also configure the simple builder settings and the `ISimpleBuilder` instance service lifetime.

```c#
services.AddSimpleSqlBuilder(
    serviceLifeTime = ServiceLifetime.Scoped, // Optional. Default is ServiceLifetime.Singleton
    configure =>
    {
        configure.DatabaseParameterNameTemplate = "param"; // Optional. Default is "p"
        configure.DatabaseParameterPrefix = ":"; // Optional. Default is "@"
        configure.ReuseParameters = true; // Optional. Default is "false"
        configure.UseLowerCaseClauses = true; // Optional. Default is "false". This is only applicable to the fluent builder
    });
```

The settings can also be configured per simple builder instance if you want to override the global settings.

## Database Support

The library supports any database that Dapper supports. However, the library has been tested against MSSQL, MySQL and PostgreSQL databases. The integration test can be found here [SimpleSqlBuilder.IntegrationTests](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Tests/IntegrationTests/SimpleSqlBuilder.IntegrationTests).

## Benchmark

The benchmark below shows the performance of the `Builder` and `FluentBuilder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark SQL execution**).

``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.308
  [Host]     : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-GEPLMO : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  Job-FSXWVC : .NET Framework 4.8.1 (4.8.9105.0), X64 RyuJIT VectorSize=256


```

|                             Method |              Runtime |   Categories |      Mean | Allocated |
|----------------------------------- |--------------------- |------------- |----------:|----------:|
|                SqlBuilder (Dapper) |             .NET 6.0 | Simple query |  1.922 μs |   2.91 KB |
|                            Builder |             .NET 6.0 | Simple query |  1.647 μs |   5.05 KB |
|                      FluentBuilder |             .NET 6.0 | Simple query |  2.017 μs |   4.57 KB |
|         Builder (Reuse parameters) |             .NET 6.0 | Simple query |  2.356 μs |   5.38 KB |
|   FluentBuilder (Reuse parameters) |             .NET 6.0 | Simple query |  2.727 μs |    4.9 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 | Simple query |  3.310 μs |   3.43 KB |
|                            Builder | .NET Framework 4.6.1 | Simple query |  4.373 μs |   5.55 KB |
|                      FluentBuilder | .NET Framework 4.6.1 | Simple query |  4.475 μs |    5.2 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.205 μs |   6.12 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 | Simple query |  5.288 μs |   5.77 KB |
|                                    |                      |              |           |           |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) |             .NET 6.0 |  Large query | 28.648 μs |  42.19 KB |
|                            Builder |             .NET 6.0 |  Large query | 23.737 μs |  66.15 KB |
|                      FluentBuilder |             .NET 6.0 |  Large query | 28.695 μs |  49.73 KB |
|         Builder (Reuse parameters) |             .NET 6.0 |  Large query | 17.750 μs |  46.76 KB |
|   FluentBuilder (Reuse parameters) |             .NET 6.0 |  Large query | 22.434 μs |  30.34 KB |
|                                    |                      |              |           |           |
|                SqlBuilder (Dapper) | .NET Framework 4.6.1 |  Large query | 44.050 μs |  53.09 KB |
|                            Builder | .NET Framework 4.6.1 |  Large query | 62.126 μs |  74.55 KB |
|                      FluentBuilder | .NET Framework 4.6.1 |  Large query | 65.133 μs |  68.61 KB |
|         Builder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 48.110 μs |  49.83 KB |
|   FluentBuilder (Reuse parameters) | .NET Framework 4.6.1 |  Large query | 50.751 μs |  43.87 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.

## Contributing

Refer to the [Contributing](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CONTRIBUTING.md) guide for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/LICENSE.md) file for details.
