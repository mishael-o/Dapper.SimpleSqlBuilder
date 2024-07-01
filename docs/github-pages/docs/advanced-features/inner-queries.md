# Subqueries and inner queries

There might be times when you need to write a query that contains a subquery or inner query. The library supports this by allowing you to pass a [formattable string](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) to the builders, facilitating the breakdown of complex queries into smaller, manageable parts. See the example below.

```csharp
int userTypeId = 10;
// Define a subquery using FormattableString
FormattableString subQuery = $"SELECT Type FROM UserType WHERE Id = {userTypeId}";

// Use the subquery within a larger query
var builder = SimpleBuilder.Create($@"
SELECT x.*, ({subQuery}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type FROM UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p1
```
