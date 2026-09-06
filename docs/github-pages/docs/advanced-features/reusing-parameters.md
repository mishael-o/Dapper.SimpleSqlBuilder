# Reusing Parameters

The library supports reusing the same parameter name for parameters that have the same value, type, and properties. By default, this feature is disabled, but it can be enabled globally through [Builder Settings](../configuration/builder-settings.md) or [Builder Options](../configuration/dependency-injection.md#configuring-builder-options) when using [dependency injection](../configuration/dependency-injection.md). It can also be configured on a per-builder instance basis.

> [!NOTE]
> Parameter reuse does not apply to `null` values.

The example below demonstrates how to enable this feature:

```csharp
int maxAge = 30;
int userTypeId = 10;

// Configuring globally for all builder instances
SimpleBuilderSettings.Configure(reuseParameters: true);

var builder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type FROM UserType WHERE Id = {userTypeId}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}");

// OR

// Configuring per-builder instance
builder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type FROM UserType WHERE Id = {userTypeId}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}",
reuseParameters: true);
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1
```

## Defined Parameters

The `reuseParameters` setting applies to interpolated values. A parameter you define yourself with [`DefineParam`](parameter-properties.md) carries its own setting and is reused by default, because defining a parameter and using it in more than one place describes a single parameter.

```csharp
var userTypeId = 10.DefineParam(DbType.Int32);

// reuseParameters is not enabled, yet the defined parameter is still reused
var builder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type FROM UserType WHERE Id = {userTypeId}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}");
```

```sql
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
```

Pass `reuse: false` to get a separate parameter for each use:

```csharp
var userTypeId = 10.DefineParam(DbType.Int32, reuse: false);
```

```sql
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p1
```

The two settings are independent, so `reuse: false` is honoured even when `reuseParameters` is enabled, and a defined parameter is reused even when it is disabled.
