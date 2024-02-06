# Subqueries and inner queries

The might be times when you need to write a query that contains a subquery or inner query. The library supports this by allowing you to pass a [formattable strings](https://docs.microsoft.com/en-us/dotnet/api/system.formattablestring) to the builders. This is useful when you want to break complex queries into smaller ones. See the example below.

```csharp
int userTypeId = 10;
FormattableString subQuery = $"SELECT Type from UserType WHERE Id = {userTypeId}";

var builder = SimpleBuilder.Create($@"
SELECT x.*, ({subQuery}) AS UserType
FROM User x
WHERE UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT x.*, (SELECT Type from UserType WHERE Id = @p0) AS UserType
FROM User x
WHERE UserTypeId = @p1
```
