# Lower Case Clauses

The [Fluent Builder](fluent-builder.md) supports generating SQL with lower case clauses. The example below shows how to generate SQL with lower case clauses.

```csharp
// Configuring globally. Can also be configured per Fluent Builder instance.
SimpleBuilderSettings.Configure(useLowerCaseClauses: true);

int userTypeId = 1;
int minAge = 20;
int maxAge = 50;

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

The generated SQL will be:

```sql
select u.Role, u.Age, ut.Type, COUNT(u.Id) AS UserCount
from User u
inner join UserType ut ON u.UserTypeId = ut.Id
where u.Role IN @p0 or u.UserTypeId = @p1
group by u.Role, u.Age, ut.Type
having u.Age >= @p2 and u.Age < @p3
order by u.Role ASC
```
