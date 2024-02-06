# Quick Start

A simple SQL builder for [Dapper](https://github.com/DapperLib/Dapper) using [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) and fluent API for building safe static and dynamic SQL queries.

## Installation

The example below shows how to install the [Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) package. To install other packages, see the [Packages](docs/introduction.md#packages) section.

Install via the NuGet Package Manager Console

```powershell
Install-Package Dapper.SimpleSqlBuilder
```

Or via the .NET Core command line interface

```bash
dotnet add package Dapper.SimpleSqlBuilder
```

## Usage

The library provides two builders for building SQL queries, which can be created via the static `SimpleBuilder` class.

- `Builder` - for building static, dynamic, and complex SQL queries.
- `Fluent Builder` - for building SQL queries using fluent API.

 The library also provides an alternative to static classes via [dependency injection](docs/configuration/dependency-injection.md).

### Create SQL query with the `Builder`

```csharp
using Dapper.SimpleSqlBuilder;

var userTypeId = 4;
var role = "Admin";

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND Role = {role}");
```

> [!NOTE]
> The concern you might have here is the issue of SQL injection, however this is mitigated by the library as the SQL statement is converted to this.

```sql
SELECT * FROM User
WHERE Id = @p0 AND Role = @p1
```

> And All values passed into the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's [DynamicParameters](https://github.com/DapperLib/Dapper/blob/main/Dapper/DynamicParameters.cs) collection.

To execute the query with Dapper is as simple as this:

```csharp
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

To learn more about the builder, see the [Builder](docs/builders/builder.md) section.

### Create SQL query with the `Fluent Builder`

```csharp
using Dapper.SimpleSqlBuilder;

var userTypeId = 4;
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

To learn more about the fluent builder, see the [Fluent Builder](docs/builders/fluent-builder/fluent-builder.md) section.

### Simple Builder Settings

To learn about configuring the simple builder, see the [Builder Settings](docs/configuration/builder-settings.md) section.

## Next Steps

- [Introduction](docs/introduction.md)
