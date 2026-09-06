---
title: Quick Start
_disableBreadcrumb: true
---

<div class="homepage-hero">
  <div class="homepage-hero__header">
    <img class="homepage-hero__logo" src="images/logo.svg" alt="Dapper.SimpleSqlBuilder logo">
    <p class="homepage-hero__eyebrow">Safe, parameterized SQL for Dapper</p>
  </div>

  <p class="homepage-hero__lead">A simple and <a href="docs/miscellaneous/performance.md">performant</a> SQL builder for <a href="https://github.com/DapperLib/Dapper">Dapper</a>, using <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated">string interpolation</a> and a fluent API to build safe, static, and dynamic SQL queries, keeping your parameters explicit and your query building experience ergonomic.</p>
  <div class="hero-demo">
    <figure class="hero-demo__panel hero-demo__panel--in">
      <figcaption class="hero-demo__label">You write</figcaption>
      <pre class="hero-demo__code"><span class="hero-demo__tok"><span class="tok-op">$@"</span><span class="tok-kw">SELECT</span> <span class="tok-op">*</span> <span class="tok-kw">FROM</span> User</span>
<span class="hero-demo__tok"><span class="tok-kw">WHERE</span> Role</span> <span class="hero-demo__tok"><span class="tok-op">=</span></span> <span class="hero-demo__tok tok-slot">{role}</span><span class="tok-op">"</span></pre>
    </figure>
    <div class="hero-demo__joint" aria-hidden="true">
      <svg class="hero-demo__arrow" viewBox="0 0 16 16" fill="none"><path d="M1 8h12M9 4l4 4-4 4" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/></svg>
    </div>
    <figure class="hero-demo__panel hero-demo__panel--out">
      <figcaption class="hero-demo__label">Dapper receives</figcaption>
      <pre class="hero-demo__code"><span class="hero-demo__tok"><span class="tok-kw">SELECT</span> <span class="tok-op">*</span> <span class="tok-kw">FROM</span> User</span>
<span class="hero-demo__tok"><span class="tok-kw">WHERE</span> Role</span> <span class="hero-demo__tok"><span class="tok-op">=</span></span> <span class="hero-demo__tok tok-slot">@p0</span></pre>
    </figure>
  </div>
  <p class="hero-demo__note">The value never touches the SQL. It goes to Dapper as a parameter.</p>
  <div class="homepage-hero__actions">
    <a class="btn btn-primary btn-lg" href="api-docs/netcore/Dapper.SimpleSqlBuilder.yml">Browse API</a>
    <a class="btn btn-outline-secondary btn-lg" href="https://www.nuget.org/packages/Dapper.SimpleSqlBuilder">NuGet</a>
  </div>
</div>

## Installation

The example below shows how to install the [Dapper.SimpleSqlBuilder](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder) package. To install other packages, see the [Packages](docs/introduction.md#packages) section.

Install via the .NET Core command line interface

```bash
dotnet add package Dapper.SimpleSqlBuilder
```

Or via the NuGet Package Manager Console

```powershell
Install-Package Dapper.SimpleSqlBuilder
```

## Usage

The library provides two builders for building SQL queries, which can be created via the static `SimpleBuilder` class.

- `Builder` - for building static, dynamic, and complex SQL queries.
- `Fluent Builder` - for building SQL queries using a fluent API.

 The library also provides an alternative to static classes via [Dependency Injection](docs/configuration/dependency-injection.md).

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
WHERE UserTypeId = @p0 AND Role = @p1
```

> And all values passed into the interpolated string are taken out and replaced with parameter placeholders. The parameter values are put into Dapper's <xref:Dapper.DynamicParameters> collection.

To execute the query with Dapper is as simple as this:

```csharp
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

See the [Builder](docs/builders/builder.md) section for more information.

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
WHERE UserTypeId = @p0 AND Role IN @pc1_
```

> [!NOTE]
> When the query is executed, Dapper will expand the parameter `pc1_` into individual parameters (`pc1_1`, `pc1_2`, etc.) for each value in the collection.

See the [Fluent Builder](docs/builders/fluent-builder/fluent-builder.md) section for more information.

### Builder Settings

See the [Builder Settings](docs/configuration/builder-settings.md) section to learn about configuring the builders.

## Next Steps

<div class="homepage-card-grid">
  <a class="homepage-card" href="docs/introduction.md">
    <p class="homepage-card__title">Introduction</p>
    <p>Learn the core concepts and see which package fits your project.</p>
  </a>
  <a class="homepage-card" href="docs/builders/builder.md">
    <p class="homepage-card__title">Builder</p>
    <p>Create static, conditional, and complex SQL with plain interpolated strings.</p>
  </a>
  <a class="homepage-card" href="docs/builders/fluent-builder/fluent-builder.md">
    <p class="homepage-card__title">Fluent Builder</p>
    <p>Chain SQL clauses with a readable API that stays parameterized by default.</p>
  </a>
  <a class="homepage-card" href="docs/miscellaneous/performance.md">
    <p class="homepage-card__title">Performance</p>
    <p>See benchmark data and why builder ergonomics do not have to cost throughput.</p>
  </a>
</div>
