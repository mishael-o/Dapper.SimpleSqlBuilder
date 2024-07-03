# Introduction

**Dapper.SimpleSqlBuilder** enhances the [Dapper](https://github.com/DapperLib/Dapper) experience by simplifying the construction of both static and dynamic SQL queries. Leveraging [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) and a fluent API, it enables developers to easily create safe, parameterized SQL queries. This efficiency is achieved by utilizing [FormattableString](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) and [interpolated string handlers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/interpolated-string-handler), ensuring that parameters are securely captured.

> [!NOTE]
> The library provides a feature set for building and parametrizing SQL queries, however all of Dapper's features and quirks still apply for query parameters.

## Key Features

- Provides a simple and natural way to write SQL queries using string interpolation.
- Chainable methods and fluent APIs for building SQL queries.
- Supports parameter reuse within queries.
- Dependency injection support.
- Conditional methods for building dynamic SQL queries.
- [Performant and memory efficient](miscellaneous/performance.md). Performs similarly or better when compared to Dapper's [SqlBuilder](https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder).

The library provides two builders for building SQL queries:

- [Builder](builders/builder.md) - for building static, dynamic and complex SQL queries.
- [Fluent Builder](builders/fluent-builder/fluent-builder.md) - for building dynamic SQL queries using a fluent API.

## Packages

The library provides multiple packages to suit your needs.

[Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder): A simple SQL builder for Dapper, using string interpolation and a fluent API to build safe, static, and dynamic SQL queries.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder)

[Dapper.SimpleSqlBuilder.StrongName](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName): A package that uses Dapper.StrongName.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.StrongName?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.StrongName)

[Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection): Dependency injection extension for Dapper.SimpleSqlBuilder.

[![Nuget](https://img.shields.io/nuget/v/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) [![Nuget](https://img.shields.io/nuget/dt/Dapper.SimpleSqlBuilder.DependencyInjection?logo=nuget)](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection)

## Database Support

The library supports any database that Dapper supports. However, the library has been [tested](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/tree/main/src/Tests/IntegrationTests/SimpleSqlBuilder.IntegrationTests) against the latest versions of MSSQL, MySQL and PostgreSQL databases.

## Share Your Feedback

If you like the library, use it, share it, and give it a ⭐️ on GitHub. For any suggestions, feature requests, or issues feel free to create an [issue](https://github.com/mishael-o/Dapper.SimpleSqlBuilder/issues) on GitHub to help improve the library.

## Next Steps

- [Quick Start](../index.md)
- [Release Notes](miscellaneous/release-notes.md)
