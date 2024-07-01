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
var secondBuilder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1",
reuseParameters: true);
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1
```
