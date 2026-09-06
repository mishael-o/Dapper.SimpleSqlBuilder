# Dapper Simple SQL Builder

[![Continuous integration and delivery](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/ci-cd.yml) [![CodeQL](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/codeql.yml/badge.svg)](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/actions/workflows/codeql.yml) [![Codecov](https://img.shields.io/codecov/c/gh/mishael-o/Dapper.SimpleSqlBuilder?logo=codecov)](https://codecov.io/gh/mishael-o/Dapper.SimpleSqlBuilder)

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder?logo=nuget&label=Dapper.SimpleSqlBuilder)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) [![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.StrongName?logo=nuget&label=Dapper.SimpleSqlBuilder.StrongName)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName) [![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget&label=Dapper.SimpleSqlBuilder.DependencyInjection)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection)

![Dapper Simple SQL Builder](https://raw.githubusercontent.com/mishael-o/Dapper.SimpleSqlBuilder/main/images/readme-icon.png)

A simple and [performant](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/miscellaneous/performance) SQL builder for [Dapper](https://github.com/DapperLib/Dapper), using [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) and a fluent API to build safe, static, and dynamic SQL queries.

## Getting Started

Install via the .NET Core command line interface

```bash
dotnet add package Dapper.SimpleSqlBuilder
```

Or via the NuGet Package Manager Console

```powershell
Install-Package Dapper.SimpleSqlBuilder
```

For information on installing other available packages, refer to the [Packages](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/introduction.html#packages) section in the documentation.

### Usage

The library provides two builders for building SQL queries, which can be created via the static `SimpleBuilder` class.

- `Builder` - for building static, dynamic, and complex SQL queries.
- `Fluent Builder` - for building SQL queries using a fluent API.

 The library also provides an alternative to static classes via [Dependency Injection](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/configuration/dependency-injection.html).

#### Create SQL query with the `Builder`

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
WHERE UserTypeId = @p0 AND Role = @p1
```

> And all values passed into the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's [DynamicParameters](https://github.com/DapperLib/Dapper/blob/main/Dapper/DynamicParameters.cs) collection.

To execute the query with Dapper is as simple as this:

```csharp
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

To learn more about the `Builder`, refer to the [Builder](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/builders/builder.html) section in the documentation.

#### Create SQL query with the `Fluent Builder`

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
WHERE UserTypeId = @p0 AND Role IN @pc1_
```

> [!NOTE]
> When the query is executed, Dapper will expand the parameter `pc1_` into individual parameters (`pc1_1`, `pc1_2`, etc.) for each value in the collection.

To learn more about the `Fluent Builder`, refer to the [Fluent Builder](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/builders/fluent-builder/fluent-builder.html) section in the documentation.

### The Docs 📚

For advanced configuration options including parameter naming conventions, prefixes, and other settings, visit the [Builder Settings](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/configuration/builder-settings.html) section in the documentation.

Explore the complete [Documentation](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/introduction.html) to learn more about the library's features and usage.

## Share Your Feedback

If you like the library, use it, share it, and give it a ⭐️. For any suggestions, feature requests, or issues feel free to create an [issue](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/issues) to help improve the library.

## Contributing

Refer to the [Contributing](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/docs/CONTRIBUTING.md) guide for more details.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/blob/main/LICENSE.md) file for details.

## Acknowledgements

Refer to the [Acknowledgements](https://mishael-o.github.io/Dapper.SimpleSqlBuilder/docs/miscellaneous/acknowledgements) page for more details.
