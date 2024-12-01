# Parameter Properties

The library enables you to configure parameter properties via the `AddParameter` method. For example, you may want to define a parameter's <xref:System.Data.DbType>, as shown below.

```csharp
int id = 10;

var builder = SimpleBuilder.Create($"SELECT * FROM User WHERE Id = @id")
    .AddParameter("id", value: id, dbType: DbType.Int32);
```

However, the library also provides an extension method, [`DefineParam`](xref:Dapper.SimpleSqlBuilder.Extensions.SimpleParameterInfoExtensions.DefineParam``1(``0,System.Nullable{System.Data.DbType},System.Nullable{System.Int32},System.Nullable{System.Byte},System.Nullable{System.Byte})), to streamline this process when using interpolated strings.

```csharp
using Dapper.SimpleSqlBuilder.Extensions;

int id = 10;

// Define parameter properties
var idParam = id.DefineParam(dbType: DbType.Int32);
var builder = SimpleBuilder.Create($"SELECT * FROM User WHERE Id = {idParam}");

// OR

// Defining parameter properties in-line
var fluentBuilder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Id = {id.DefineParam(dbType: DbType.Int32)}");
```

The [`DefineParam`](xref:Dapper.SimpleSqlBuilder.Extensions.SimpleParameterInfoExtensions.DefineParam``1(``0,System.Nullable{System.Data.DbType},System.Nullable{System.Int32},System.Nullable{System.Byte},System.Nullable{System.Byte})) extension method enables you to define the [`DbType`](xref:System.Data.DbType), `Size`, `Precision` and `Scale` of your parameter. This should only be used for parameters passed into the interpolated string, as the <xref:System.Data.ParameterDirection> is always set to `Input` for values in the interpolated string.

For cases where you don't want to use the extension method, you can manually create the parameter object:

```csharp
var idParam = new SimpleParameterInfo(id, dbType: DbType.Int64);
```
