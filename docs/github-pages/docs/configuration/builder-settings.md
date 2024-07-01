# Builder Settings

You can configure the builder settings through the [`SimpleBuilderSettings`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.SimpleBuilderSettings.yml) static class by calling the `Configure` method.

> [!NOTE]
> If you are using the dependency injection library, refer to the [Dependency Injection](dependency-injection.md) section on how to configure the global builder settings.

The code below shows how to configure the builder settings.

```csharp
SimpleBuilderSettings.Configure
(
    parameterNameTemplate: "param", // Optional. The default is "p"
    parameterPrefix: ":", // Optional. The default is "@"
    reuseParameters: true, // Optional. The default is false
    useLowerCaseClauses: true // Optional. The default is false. This is only applicable to the fluent builder.
);
```

## Configuring Parameter Name Template

The default parameter name template is `p`, meaning when parameters are created, they will be named `p0`, `p1`, `p2`, etc. You can configure this by passing your desired value to the `parameterNameTemplate` parameter.

```csharp
SimpleBuilderSettings.Configure(parameterNameTemplate: "param");
```

## Configuring Parameter Prefix

The default parameter prefix is `@`, meaning when parameters are passed to the database they will be passed as `@p0`, `@p1`, `@p2`, etc. However, this may not be applicable to all databases. You can configure this by passing your desired value to the `parameterPrefix` parameter.

```csharp
SimpleBuilderSettings.Configure(parameterPrefix: ":");
```

This can also be configured per builder instance if you want to override the global settings.

```csharp
// Builder
var builder = SimpleBuilder.Create(parameterPrefix: ":");

// Fluent builder
var fluentBuilder = SimpleBuilder.CreateFluent(parameterPrefix: ":");
```

### Configuring Parameter Reuse

The library supports parameter reuse, and the default is `false`. Go to the [Reusing Parameters](../advanced-features/reusing-parameters.md) section to learn more. You can configure this by passing your desired argument to the `reuseParameters` parameter.

```csharp
SimpleBuilderSettings.Configure(reuseParameters: true);
```

This can also be configured per builder instance if you want to override the global settings.

```csharp
// Builder
var builder = SimpleBuilder.Create(reuseParameters: true);

// Fluent builder
var fluentBuilder = SimpleBuilder.CreateFluent(reuseParameters: true);
```

### Configuring Fluent builder to use Lower Case Clauses

The library supports using lower case clauses for the [fluent builder](../builders/fluent-builder/fluent-builder.md), and the default is `false`. You can configure this by passing your desired argument to the `useLowerCaseClauses` parameter.

```csharp
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);
```

This can also be configured per fluent builder instance if you want to override the global settings.

```csharp
var builder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true);
```
