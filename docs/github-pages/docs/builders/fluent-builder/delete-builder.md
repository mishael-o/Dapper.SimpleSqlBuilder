# Delete Builder

You can perform `DELETE` operations with the [`Delete Builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IDeleteBuilderEntry) as seen in the example below.

```csharp
int id = 10;

var builder = SimpleBuilder.CreateFluent()
    .DeleteFrom($"User")
    .Where($"Id = {id}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
DELETE FROM User
WHERE Id = @p0
```

For complex `WHERE` clause statements refer to the [Where Filters](where-filters.md) section.
