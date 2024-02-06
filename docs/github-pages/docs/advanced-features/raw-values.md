# Raw Values

There are scenarios where you may want to pass a raw value into the interpolated string and not parameterize the value. The `raw` format string is used to indicate that the value should not be parameterized.

> [!WARNING]
> Do not use raw values if you don't trust the source or have not sanitized your value, as this can lead to SQL injection.

## Column and Table names with `nameof()`

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"{nameof(User.Id):raw}, {nameof(User.FirstName):raw}, {nameof(User.LastName):raw}")
    .From($"{nameof(User):raw}");
```

The generated SQL will be:

```sql
SELECT Id, FirstName, LastName
FROM User
```

### Named Parameter

This example uses T-SQL (MSSQL) syntax, however the same applies to other databases.

```csharp
const string idParamName = "id";
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($@"
BEGIN
    SELECT @{idParamName:raw} = NEXT VALUE FOR UserSequence;
    INSERT INTO User (Id, FirstName, LastName, UserTypeId)
    VALUES (@{idParamName:raw}, {user.FirstName}, {user.LastName}, {user.UserTypeId});
END")
.AddParameter(idParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

dbConnection.Execute(builder.Sql, builder.Parameters);
var id = builder.GetValue<int>(idParamName);
```

The generated SQL will be:

```sql
BEGIN
    SELECT @id = NEXT VALUE FOR UserSequence;
    INSERT INTO User (Id, FirstName, LastName, UserTypeId)
    VALUES (@id, @p0, @p1, @p2);
END
```
