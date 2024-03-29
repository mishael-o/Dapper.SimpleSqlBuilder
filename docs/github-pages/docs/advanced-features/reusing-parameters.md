# Reusing Parameters

The library supports reusing the same parameter name for parameters with the same value, type, and properties. This is turned off by default, however can be enabled globally via the [Builder Settings](../configuration/builder-settings.md) or [Builder Options](../configuration/dependency-injection.md#configuring-builder-options) (when using [dependency injection](../configuration/dependency-injection.md)). This can also configured per builder instance.

> [!NOTE]
> Parameter reuse does not apply to `null` values.

The example below shows how.

```csharp
// Configuring globally. Can also be configured per builder instance.
SimpleBuilderSettings.Configure(reuseParameters: true);

int maxAge = 30;
int userTypeId = 10;

var builder = SimpleBuilder.Create($@"
SELECT x.*, (SELECT Type from UserType WHERE Id = {userTypeId}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}
AND Age <= {maxAge}");
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type from UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p0
AND Age <= @p1
```
