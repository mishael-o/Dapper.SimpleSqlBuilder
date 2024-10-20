# Select Builder

You can perform `SELECT` operations with the [`Select Builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ISelectBuilderEntry) as seen in the examples below.

## Select

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Role")
    .From($"User");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName").Select($"LastName").Select($"Role")
    .From($"User");

// Execute the query with Dapper
var users = dbConnection.Query<User>(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Role
FROM User
```

**Another example:**

```csharp
string role = "Admin%";
int userTypeId = 10;

var builder = SimpleBuilder.CreateFluent()
    .Select($"*")
    .From($"User")
    .Where($"Role LIKE {role}")
    .OrWhere($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT *
FROM User
WHERE Role LIKE @p0 OR UserTypeId = @p1
```

For complex `WHERE` clause statements refer to the [Where Filters](where-filters.md) section.

## Select Distinct

```csharp
var builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"UserTypeId, Role")
    .From($"User");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .SelectDistinct($"UserTypeId").SelectDistinct($"Role")
    .From($"User");
```

The generated SQL will be:

```sql
SELECT DISTINCT UserTypeId, Role
FROM User
```

## Join

```csharp
int minAge = 18;

var builder = SimpleBuilder.CreateFluent()
    .Select($"u.FirstName, u.LastName, u.Age, ut.Type, us.Status, ua.AddressLine1, ua.AddressLine2")
    .From($"User u")
    .InnerJoin($"UserType ut ON u.UserTypeId = ut.Id")
    .RightJoin($"UserStatus us ON u.UserStatusId = us.Id")
    .LeftJoin($"UserAddress ua ON u.UserAddressId = ua.Id")
    .Where($"u.Age >= {minAge}");
```

The generated SQL will be:

```sql
SELECT u.FirstName, u.LastName, u.Age, ut.Type, us.Status, ua.AddressLine1, ua.AddressLine2
FROM User u
INNER JOIN UserType ut ON u.UserTypeId = ut.Id
RIGHT JOIN UserStatus us ON u.UserStatusId = us.Id
LEFT JOIN UserAddress ua ON u.UserAddressId = ua.Id
WHERE u.Age >= @p0
```

## Group By

```csharp
var roles = new[] { "Admin", "User" };

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, UserTypeId, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role NOT IN {roles}")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role, UserTypeId");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"Role, UserTypeId, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role NOT IN {roles}")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role").GroupBy($"UserTypeId");
```

The generated SQL will be:

```sql
SELECT Role, UserTypeId, COUNT(Id) AS UserCount
FROM User
WHERE Role NOT IN @p0 AND Role IS NOT NULL
GROUP BY Role, UserTypeId
```

## Having

```csharp
int minAge = 18;

var builder = SimpleBuilder.CreateFluent()
    .Select($"Role, Age, COUNT(Id) AS UserCount")
    .From($"User")
    .Where($"Role IS NOT NULL")
    .GroupBy($"Role, Age")
    .Having($"COUNT(Id) > 1").Having($"Age >= {minAge}");
```

The generated SQL will be:

```sql
SELECT Role, Age COUNT(Id) AS UserCount
FROM User
WHERE Role IS NOT NULL
GROUP BY Role, Age
HAVING COUNT(Id) > 1 AND Age >= @p0
```

## Order By

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName")
    .From($"User")
    .OrderBy($"FirstName ASC, LastName DESC");

// The query can also be written as this
builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName")
    .From($"User")
    .OrderBy($"FirstName ASC").OrderBy($"LastName DESC");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName
FROM User
ORDER BY FirstName ASC, LastName DESC
```

## Pagination

The [`Select Builder`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ISelectBuilderEntry) supports two popular ways of performing pagination. The choice between `Limit`/`Offset` and `OffsetRows`/`FetchNext` methods may depend on your database system.

### [`Limit`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.ILimitBuilder.Limit(System.Int32)) and [`Offset`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IOffsetBuilder.Offset(System.Int32))

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age")
    .From($"User")
    .OrderBy($"Age ASC")
    .Limit(10)
    .Offset(20);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age
FROM User
ORDER BY Age ASC
LIMIT 10 OFFSET 20
```

### [`OffsetRows`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IOffsetRowsBuilder.OffsetRows(System.Int32)) and [`FetchNext`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IFetchBuilder.FetchNext(System.Int32))

```csharp
var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age")
    .From($"User")
    .OrderBy($"Age ASC")
    .OffsetRows(20)
    .FetchNext(10);
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age
FROM User
ORDER BY Age ASC
OFFSET 20 ROWS 
FETCH NEXT 10 ROWS ONLY
```
