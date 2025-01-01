# Builder Settings

You can configure the builder settings through the [`SimpleBuilderSettings`](xref:Dapper.SimpleSqlBuilder.SimpleBuilderSettings) static class by calling the `Configure` method.

> [!NOTE]
> If you are using the dependency injection library, refer to the [Dependency Injection](dependency-injection.md) section on how to configure the global builder settings.

The code below shows how to configure the builder settings.

```csharp
SimpleBuilderSettings.Configure
(
    parameterNameTemplate: "param", // Optional. The default is "p"
    parameterPrefix: ":", // Optional. The default is "@"
    collectionParameterTemplateFormat: "col{0}_", // Optional. The default is "c{0}_"
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

## Configure Collection Parameter Template

When working with collections, the builder generates unique parameter names using a template format. By default, the format is `c{0}_`, where `{0}` is replaced with an index.

For example, if the `parameterNameTemplate` is configured as `p`, the generated collection parameters will be named `pc0_`, `pc1_`, `pc2_`, etc. When expanded into multiple parameters by Dapper, `pc0_` will be expanded to `pc0_1`, `pc0_2`, etc.

You can customize this format by passing your desired value to the `collectionParameterTemplateFormat` parameter:

```csharp
SimpleBuilderSettings.Configure(collectionParameterTemplateFormat: "col{0}_");
```

## Configuring Parameter Reuse

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

## Configuring Fluent builder to use Lower Case Clauses

The library supports using lower case clauses for the [fluent builder](../builders/fluent-builder/fluent-builder.md), and the default is `false`. You can configure this by passing your desired argument to the `useLowerCaseClauses` parameter.

```csharp
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);
```

This can also be configured per fluent builder instance if you want to override the global settings.

```csharp
var fluentBuilder = SimpleBuilder.CreateFluent(useLowerCaseClauses: true);
```
