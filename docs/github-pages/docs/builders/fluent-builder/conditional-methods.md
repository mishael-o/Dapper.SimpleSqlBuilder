# Conditional Methods (Clauses)

The [Fluent Builder](fluent-builder.md) supports conditional methods (clauses). This is useful when you want to add a clause only if a condition is met.

The `Set`, `InnerJoin`, `RightJoin`, `LeftJoin`, `Where`, `OrWhere`, `WithFilter`, `WithOrFilter`, `GroupBy`, `Having` and `OrderBy` methods all have conditional overloads.

## Example 1

```csharp
var users = GetUsers("John", null, "Admin", true);

IEnumerable<User> GetUsers(string firstNameSearch, int? userTypeId, string role, bool orderByFirstName)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .Where(!string.IsNullOrWhiteSpace(firstNameSearch), $"FirstName LIKE {'%' + firstNameSearch + '%'}")
        .Where(userTypeId.HasValue, $"UserTypeId = {userTypeId}")
        .Where(role != null, $"Role = {role}")
        .OrderBy(orderByFirstName, $"FirstName ASC");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE FirstName LIKE @p0 AND Role = @p1
ORDER BY FirstName ASC
```

## Example 2

```csharp
var filter = new Filter { UserTypeId = null, Roles = new [] { "Admin", "User" }, IncludeUsersWithoutRole = true };
var users = GetUsers(filter);

IEnumerable<User> GetUsers(Filter filter)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .Where(filter?.UserTypeId.HasValue == true, $"UserTypeId = {filter.UserTypeId}")
        .OrWhere(filter?.Roles?.Length > 0, $"Role IN {filter.Roles}")
        .OrWhere(filter?.IncludeUsersWithoutRole == true, $"Role IS NULL");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE Role IN @p0 OR Role IS NULL
```

## Example 3

```csharp
var result = UpdateUser(10, "John", "Doe", null, 30);

bool UpdateUser(int id, string firstName, string lastName, string role, int? userTypeId)
{
    var builder = SimpleBuilder.CreateFluent()
        .Update($"User")   
        .Set($"FirstName = {firstName}")
        .Set($"LastName = {lastName}")
        .Set(role is not null, $"Role = {role}")
        .Set(userTypeId.HasValue, $"UserTypeId = {userTypeId}")
        .Where($"Id = {id}");

    return dbConnection.Execute(builder.Sql, builder.Parameters) > 0;
}
```

The generated SQL will be:

```sql
UPDATE User
SET FirstName = @p0, LastName = @p1, UserTypeId = @p2
WHERE Id = @p3
```

## Example 4

```csharp
var roles = new[] { "Admin", "User" };
var userTypeIds = new[] { 1, 2, 3 };

var users = GetUsers(roles, false, userTypeIds, true);

IEnumerable<User> GetUsers(string[] roles, bool includeUsersWithoutRole, int[] userTypeIds, bool includeUsersWithoutUserTypeId)
{
    var builder = SimpleBuilder.CreateFluent()
        .Select($"*")
        .From($"User")
        .WhereFilter()
            .WithFilter(roles?.Length > 0, $"Role IN {roles}")
            .WithFilter(includeUsersWithoutRole, $"Role IS NULL")
        .OrWhereFilter()
            .WithFilter(userTypeIds?.Length > 0, $"UserTypeId IN {userTypeIds}")
            .WithOrFilter(includeUsersWithoutUserTypeId, $"UserTypeId IS NULL");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE (Role IN @p0) OR (UserTypeId IN @p1)
```
