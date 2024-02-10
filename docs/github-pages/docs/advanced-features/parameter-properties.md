# Parameter Properties

The library enables you to configure parameter properties via the `AddParameter` method. For example, you may want to define a <xref:System.Data.DbType> for a parameter, and the code below is how you will do this.

```csharp
int id = 10;

var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = @id")
    .AddParameter("id", value: id, dbType: DbType.Int32);
```

However, the library also provides an extension method to easily achieve this while using interpolated strings.

```csharp
using Dapper.SimpleSqlBuilder.Extensions;

// Define parameter properties
var idParam = id.DefineParam(dbType: DbType.Int32);
var builder = SimpleBuilder.Create($"SELECT * FROM User Where Id = {idParam}");

// OR

// Defining parameter properties in-line
var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Id = {id.DefineParam(dbType: DbType.Int32)}");
```

The [`DefineParam`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Extensions.SimpleParameterInfoExtensions.yml#Dapper_SimpleSqlBuilder_Extensions_SimpleParameterInfoExtensions_DefineParam__1___0_System_Nullable_System_Data_DbType__System_Nullable_System_Int32__System_Nullable_System_Byte__System_Nullable_System_Byte__) extension method enables you to define the `DbType`, `Size`, `Precision` and `Scale` of your parameter. This should only be used for parameters passed into the interpolated string, as the <xref:System.Data.ParameterDirection> is always set to `Input` for values in the interpolated string.

As an alternative to the extension method you can manually create the parameter object.

```csharp
var idParam = new SimpleParameterInfo(id, dbType: DbType.Int64);
```
