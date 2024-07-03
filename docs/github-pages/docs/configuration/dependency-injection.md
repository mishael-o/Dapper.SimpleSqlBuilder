# Dependency Injection

An alternative to using the static classes to access the builders and settings is via dependency injection. Use the [Dapper.SimpleSqlBuilder.DependencyInjection](https://www.nuget.org/packages/Dapper.SimpleSqlBuilder.DependencyInjection) NuGet package instead of the default package. The library supports the default dependency injection pattern in .NET Core.

```csharp
using Dapper.SimpleSqlBuilder.DependencyInjection;

services.AddSimpleSqlBuilder();
```

Usage in a class.

```csharp
class MyClass
{
    private readonly ISimpleBuilder simpleBuilder;

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
        var fluentBuilder = simpleBuilder.CreateFluent()
            .Select($"*")
            .From($"User")
            .Where($"Id = {id}");

        // Other code below .....
    }
}
```

## Configuring Builder Options

You can configure the builder settings and the [`ISimpleBuilder`](../../api-docs/di/Dapper.SimpleSqlBuilder.DependencyInjection.ISimpleBuilder.yml) instance service lifetime via the [`SimpleBuilderOptions`](../../api-docs/di/Dapper.SimpleSqlBuilder.DependencyInjection.SimpleBuilderOptions.yml) class. There are various ways to configure the options as shown below.

### Configuring Options via `appsettings.json`

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

```csharp
services.AddSimpleSqlBuilder(
    // Optional. Default is ServiceLifetime.Singleton
    serviceLifetime: ServiceLifetime.Singleton);
```

### Configuring Options via code

```csharp
services.AddSimpleSqlBuilder(
    configure =>
    {
        configure.DatabaseParameterNameTemplate = "param"; // Optional. The default is "p"
        configure.DatabaseParameterPrefix = ":"; // Optional. The default is "@"
        configure.ReuseParameters = true; // Optional. The default is false
        configure.UseLowerCaseClauses = true; // Optional. The default is false. This is only applicable to the fluent builder
    },
    // Optional. The default is ServiceLifetime.Singleton
    serviceLifetime: ServiceLifetime.Scoped);
```
