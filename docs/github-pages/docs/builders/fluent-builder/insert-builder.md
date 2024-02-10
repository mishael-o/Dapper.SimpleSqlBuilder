# Insert Builder

You can perform `INSERT` operations with the [`Insert Builder`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.FluentBuilder.IInsertBuilderEntry.yml) as seen in the example below.

```csharp
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.CreateFluent()
    .InsertInto($"User")
    .Columns($"FirstName, LastName, UserTypeId")
    .Values($"{user.FirstName}, {user.LastName}, {user.UserTypeId}");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
   .InsertInto($"User")
   .Columns($"FirstName").Columns($"LastName").Columns($"UserTypeId")
   .Values($"{user.FirstName}").Values($"{user.LastName}").Values($"{user.UserTypeId}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES (@p0, @p1, @p2)
```
