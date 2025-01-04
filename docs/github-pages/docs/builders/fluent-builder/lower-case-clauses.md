# Lower Case Clauses

The [fluent builder](fluent-builder.md) supports generating SQL with lower case clauses. This feature can be enabled globally or per instance, allowing for flexibility in how SQL queries are formatted.

```csharp
// Configuring globally for all builder instances
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);

int userTypeId = 1;
int minAge = 20;
int maxAge = 50;
var roles = new[] { "Admin", "User" };

var builder = SimpleBuilder.CreateFluent()
    .Select($"u.Role, u.Age, ut.Type, COUNT(u.Id) AS UserCount")
    .From($"User u")
    .InnerJoin($"UserType ut ON u.UserTypeId = ut.Id")
    .Where($"u.Role IN {roles}")
    .OrWhere($"u.UserTypeId = {userTypeId}")
    .GroupBy($"u.Role, u.Age, ut.Type")
    .Having($"u.Age >= {minAge}").Having($"u.Age < {maxAge}")
    .OrderBy($"u.Role ASC");
```

Alternatively, to configure lower case clauses for a single instance, you can pass the configuration directly to the `CreateFluent` method

The generated SQL will be:

```sql
select u.Role, u.Age, ut.Type, COUNT(u.Id) AS UserCount
from User u
inner join UserType ut ON u.UserTypeId = ut.Id
where u.Role IN @pc0_ or u.UserTypeId = @p1
group by u.Role, u.Age, ut.Type
having u.Age >= @p2 and u.Age < @p3
order by u.Role ASC
```
