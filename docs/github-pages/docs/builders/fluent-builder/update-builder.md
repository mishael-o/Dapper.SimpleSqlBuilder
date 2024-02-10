# Update Builder

You can perform `UPDATE` operations with the [`Update Builder`](../../../api-docs/netcore/Dapper.SimpleSqlBuilder.FluentBuilder.IUpdateBuilderEntry.yml) as seen in the example below.

```csharp
int id = 1;
int userTypeId = 4;
string role = "User";

var builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"UserTypeId = {userTypeId}, Role = {role}")
    .Where($"Id = {id}");

// The query can also be written as below
builder = SimpleBuilder.CreateFluent()
    .Update($"User")
    .Set($"UserTypeId = {userTypeId}")
    .Set($"Role = {role}")
    .Where($"Id = {id}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
UPDATE User
SET UserTypeId = @p0, Role = @p1
WHERE Id = @p2
```

For complex `WHERE` clause statements refer to the [Where Filters](where-filters.md) section.
