# Dapper Simple Sql Builder

[![Continuous integration and delivery](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml) [![Codecov](https://img.shields.io/codecov/c/gh/mishael-o/Dapper.SimpleSqlBuilder?logo=codecov)](https://codecov.io/gh/mishael-o/Dapper.SimpleSqlBuilder)

A simple SQL builder (that tries not to do too much 😊) for [Dapper](https://github.com/DapperLib/Dapper) using string interpolation for building dynamic sql.

This library provides a simple and easy way to build dynamic SQL and commands, that can be executed using the Dapper library. This is achieved by leveraging [FormattableString](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) to capture parameters and produce parameterized SQL.

**The library doesn't do anything special but parameterize the SQL, therefore all of Dapper's features and quirks still apply.**

## Packages

[Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder): A simple sql builder for Dapper using string interpolation.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder)

[Dapper.SimpleSqlBuilder.StrongName](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName): A package that uses Dapper.StrongName.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName)

[Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection): A dependency injection extension for .Net Core.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection)

## Simple Builder

### String Interpolation

#### Static SQL

```c#
using Dapper.SimpleSqlBuilder;

int userTypeId = 10;

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND AGE >= {25}");
```

**The concern you might have here is the issue of sql injection, however this is mitigated by the library as the SQL statement is converted to this.**

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND AGE >= @p1
```

And all values passed in the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's [DynamicParameters](https://github.com/DapperLib/Dapper/blob/main/Dapper/DynamicParameters.cs) collection.

To execute the query with Dapper is as simple as this.

```c#
dbConnection.Execute(builder.Sql, builder.Parameters);
```

#### Dynamic SQL

You can concatenate multiple interpolated strings to build your dynamic sql.

```c#
var user = new User { TypeId = 10, Role = "Admin", Age = 20 };

var builder = SimpleBuilder.Create($"SELECT * FROM User");
builder += $" WHERE UserTypeId = {user.TypeId}";

if (user.Age is not null)
{
    builder += $" AND AGE >= {user.Age}"
}
```

This will produce the sql below.

```sql
SELECT * FROM User WHERE UserTypeId = @p0 AND AGE >= @p1
```

### Builder Chaining

If you prefer an alternative to interpolated string concatenation, you can use the Append methods (`Append(...)` & `AppendNewLine(...)`) which can be chained.

```c#
int id = 10;

var builder = SimpleBuilder.Create($"SELECT * FROM User")
    .AppendNewLine($"WHERE UserTypeId = {id}")
    .Append($"AND Age >= {25}")
    .AppendNewLine($"ORDER BY Age ASC");
```

This will produce the sql below.

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

**Note**: The `Append(...)` method adds a space before the sql text by default. You can use `AppendIntact(...)` if you don't want this behaviour.

### Performing INSERT, UPDATE AND DELETE operations

#### Performing INSERT Operations

You can perform INSERT operations by passing the values in the interpolated string as seen below.

```c#
var builder = SimpleBuilder.Create($@"
INSERT INTO User (Role, Age)
VALUES ({user.Role}, {user.Age}");
```

#### Performing UPDATE Operations

You can perform UPDATE operations by passing the values in the interpolated string as seen below.

```c#
var builder = SimpleBuilder.Create($@"
UPDATE User SET Role = {user.Role}
WHERE Id = {user.Id}");
```

#### Performing DELETE Operations

You can perform DELETE operations by passing the values in interpolated string as seen below.

```c#
var builder = SimpleBuilder.Create($"DELETE FROM User WHERE Id = {user.Id}");
```

### Executing Stored Procedures

```c#
var builder = SimpleBuilder.Create($"UserResources.ProcessUserInformation")
    .AddParameter("UserRole", userRole)
    .AddParameter("UserAge", userAge)
    .AddParameter("UserId", dbType: DbType.Int32, direction: ParameterDirection.Output)
    .AddParameter("Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

dbConnection.Execute(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);
int id = builder.GetValue<int>("UserId");
int result = builder.GetValue<int>("Result");
```

### Formattable String

The library supports passing Formattable strings within Formattable strings.

```c#
int userTypeId = 10;
FormattableString subQuery = $"SELECT Description from UserType WHERE Id = {userTypeId}";

var builder = SimpleBuilder.Create(@$"
SELECT x.*, ({subQuery}) AS Description
FROM User x
WHERE UserTypeId = {userTypeId}");
```

This will create the sql below.

```sql
SELECT x.*, (SELECT Description from UserType WHERE Id = @p0) AS Description
FROM User x
WHERE UserTypeId = @p1;
```

### Parameter properties

The library enables you to configure parameter properties via the `AddParameter(...)` method. For example, you may want to define a `DbType` for a parameter, and the code below is how you might do this.

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
var builder = SimpleBuilder
    .Create($"SELECT * FROM User Where Id = {user.Id.DefineParam(dbType: DbType.Int64)}");
```

The `DefineParam(...)` extension method enables you to define the `DbType`, `Size`, `Precision` and `Scale` of your parameter. This should only be used for parameters passed into the interpolated string, as the parameter direction is always set to `Input` for values in the interpolated string.

As an alternative to the extension method you can manually create the parameter object.

```c#
var idParam = new SimpleParameterInfo(dbType: DbType.Int64);
```

## Configuring Simple Builder Settings

You can configure the simple builder settings through the `SimpleBuilderSettings` static class by calling the `Configure(...)` method. **However, if you are using the dependency injection library refer to the [Dependency Injection](#dependency-injection) section on how to configure the simple builder settings**.

The code below shows how to do this.

```c#
SimpleBuilderSettings.Configure
(
    parameterNameTemplate: "param", //Optional. Default is "p"
    parameterPrefix: ":", //Optional. Default is "@"
    reuseParameters: true // //Optional. Default is "false"
);
```

### Configuring Parameter Name Template

The default parameter name template is `p`, meaning when parameters are created they will be named `p0 p1 p2 ...` You can configure this by passing your desired value to the `parameterNameTemplate` argument.

```c#
SimpleBuilderSettings.Configure(parameterNameTemplate: "param");
```

### Configuring Parameter Prefix

The default parameter prefix is `@`, meaning when parameters are passed to the database they will be passed as `@p0 @p1 @p2 ...`, and this will not be applicable to all databases. You can configure this by passing your desired value to the `parameterPrefix` argument.

```c#
SimpleBuilderSettings.Configure(parameterPrefix: ":");
```

This can also be configured per simple builder instance if you want to override the global settings.

```c#
var builder = SimpleBuilder.Create(parameterPrefix: ":");
```

### Configuring Parameter Reuse

The library supports parameter reuse, and the default is `false`. Go to the [Reusing Parameters](#reusing-parameters) section to learn more. You can configure this by passing your desired value to the `reuseParameters` argument.

```c#
SimpleBuilderSettings.Configure(reuseParameters: true);
```

This can also be configured per simple builder instance if you want to override the global settings.

```c#
var builder = SimpleBuilder.Create(reuseParameters: true);
```

## Reusing Parameters

The library supports reusing the same parameter name for parameters with the same value, type, and properties. This is turned off by default, however can be enabled globally via the simple builder settings or per simple builder instance.

**Note:** Parameter reuse does not apply to `null` values.

See below for illustration.

```c#
// Configuring globally
SimpleBuilderSettings.Configure(reuseParameters: true);

int maxAge = 30;
int userTypeId = 10;

var builder = SimpleBuilder.Create(@$"
SELECT x.*, (SELECT Description from UserType WHERE Id = {userTypeId}) AS Description
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}");
```

The generated sql will be.

```sql
SELECT x.*, (SELECT Description from UserType WHERE Id = @p0) AS Description
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1"
```

## Raw values (:raw)

**Do not use raw values if you don't trust the source or have not sanitized your value, as this can lead to sql injection.**

There might be scenarios where you may want to pass a raw value into the interpolated string and not parameterize the value.

### Example 1.1: Dynamic Data Retrieval

```c#
IEnumerable<dynamic> GetTableData(string tableName, DateTime createDate)
{
    var builder = SimpleBuilder.Create($"SELECT * FROM {tableName:raw} WHERE CreatedDate = {createDate}");
    return dbConnection.Query(builder.Sql, builder.Parameters);
}
```

### Example 1.2: Dynamic Table Name

```c#
void CreateTable(string tableName)
{
    var builder = SimpleBuilder.Create($@"
    CREATE TABLE {tableName:raw}
    (
        Id INT PRIMARY KEY,
        Age INT NOT NULL,
        Role NVARCHAR(100) NOT NULL
    )");

    dbConnection.Execute(builder.Sql);
}
```

### Example 2 : Column names with nameof()

```c#
var builder = SimpleBuilder.Create($@"
SELECT {nameof(user.Id):raw}, {nameof(user.Age):raw}, {nameof(user.Age):raw}
FROM User");
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

        //Other code below .....
    }
}
```

### Configuring Simple Builder settings

You can also configure the simple builder settings and the `ISimpleBuilder` instance service lifetime.

```c#
services.AddSimpleSqlBuilder(
    serviceLifeTime = ServiceLifetime.Scoped, //Optional. Default is ServiceLifetime.Singleton
    configure =>
    {
        configure.DatabaseParameterNameTemplate = "param"; //Optional. Default is "p"
        configure.DatabaseParameterPrefix = ":"; //Optional. Default is "@"
        configure.ReuseParameters = true; //Optional. Default is "false"
    });
```

## Database Support

The library supports any database that Dapper supports. However, the library has been tested against MSSQL, MySQL and PostgreSQL databases. The integration test can be found here [SimpleSqlBuilder.IntegrationTests](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Tests/IntegrationTests/SimpleSqlBuilder.IntegrationTests). The tests provide real world examples of how the library can the utilised.

## Benchmark

The benchmark below shows the performance of the `SimpleSqlBuilder` compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder) for building queries only (**this does not benchmark sql execution**).

```ini
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.918/21H2)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
```

| Method                                               |       Mean | Allocated |
| ---------------------------------------------------- | ---------: | --------: |
| 'SqlBuilder (Dapper) - Simple query'                 |   1.658 μs |    2.6 KB |
| 'SimpleSqlBuilder - Simple query'                    |   1.952 μs |   4.08 KB |
| 'SimpleSqlBuilder - Simple query (Reuse parameters)' |   2.537 μs |   4.92 KB |
| 'SqlBuilder (Dapper) - Large query'                  |  84.578 μs | 274.55 KB |
| 'SimpleSqlBuilder - Large query'                     | 149.545 μs | 281.65 KB |
| 'SimpleSqlBuilder - Large query (Reuse parameters)'  | 195.550 μs | 293.99 KB |

Refer to the [benchmark project](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Benchmark/SimpleSqlBuilder.BenchMark) for more information.

## Contributing

Refer to the [Contributing](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CONTRIBUTING.md) guide for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/LICENSE.md) file for details.
