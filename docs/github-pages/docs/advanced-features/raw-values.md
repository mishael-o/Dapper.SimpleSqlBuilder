# Raw Values

In certain scenarios, you might need to insert a raw value directly into your SQL query, bypassing parameterization. The `raw` format string serves this purpose, indicating that the specified value should not be converted into a parameter.

> [!WARNING]
> Only use raw values with trusted or sanitized inputs to prevent SQL injection risks.

## Column and Table names with `nameof()`

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"{nameof(User.Id):raw}, {nameof(User.FirstName):raw}, {nameof(User.LastName):raw}")
    .From($"{nameof(User):raw}");
```

This approach generates the following SQL, leveraging `nameof()` to ensure accuracy and maintainability:

```sql
SELECT Id, FirstName, LastName
FROM User
```

### Named Parameter

This example uses T-SQL (MSSQL) syntax, but the same principles apply to other databases.

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
