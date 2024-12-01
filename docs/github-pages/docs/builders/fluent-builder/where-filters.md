# Where Filters (Complex filter statements)

The [fluent builder](fluent-builder.md) supports complex filters, allowing the addition of `WHERE`, `AND`, and `OR` clauses with complex filter statements.

## WhereFilter

The [`WhereFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereBuilder.WhereFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereFilterInterpolatedStringHandler@)) method adds a `WHERE` filter statement enclosed in parentheses to the query. Subsequent `WhereFilter` method calls add an `AND` filter statement to the query.

The `WhereFilter` method can be combined with the [`WithFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereFilterBuilder.WithFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithFilterInterpolatedStringHandler@)) and [`WithOrFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereFilterBuilder.WithOrFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithOrFilterInterpolatedStringHandler@)) methods to add `AND` and `OR` filters respectively within the filter statement.

```csharp
int minAge = 20;
int maxAge = 50;
int userTypeId = 4;

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .Where($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Age >= @p0 AND Age < @p1) AND UserTypeId = @p2
```

**Another example:**

```csharp
int minAge = 20;
int maxAge = 50;
string adminRole = "Admin";
string userRole = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .WhereFilter($"Role = {adminRole}").WithOrFilter($"Role = {userRole}").WithOrFilter($"Role IS NULL"); 
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Age >= @p0 AND Age < @p1) AND (Role = @p2 OR Role = @p3 OR Role IS NULL)
```

## OrWhereFilter

The [`OrWhereFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereBuilder.OrWhereFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereOrFilterInterpolatedStringHandler@)) method adds an `OR` filter statement enclosed in parentheses to the query.

The `OrWhereFilter` method can be combined with the [`WithFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereFilterBuilder.WithFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithFilterInterpolatedStringHandler@)) and [`WithOrFilter`](xref:Dapper.SimpleSqlBuilder.FluentBuilder.IWhereFilterBuilder.WithOrFilter(Dapper.SimpleSqlBuilder.FluentBuilder.WhereWithOrFilterInterpolatedStringHandler@)) methods to add `AND` and `OR` filters respectively within the filter statement.

```csharp
int userTypeId = 4;
int minAge = 30;
int maxAge = 65;
string role = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .Where($"UserTypeId = {userTypeId}")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhereFilter($"Role = {role}").WithOrFilter($"Role IS NULL");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE UserTypeId = @p0 OR (Age >= @p1 AND Age < @p2) OR (Role = @p3 OR Role IS NULL)
```

**Another example:**

```csharp
int minAge = 20;
int maxAge = 50;
var roles = new [] { "Admin", "User" };
string userRole = "User";

var builder = SimpleBuilder.CreateFluent()
    .Select($"FirstName, LastName, Age, Role")
    .From($"User")
    .WhereFilter($"Role IN {roles}").WithOrFilter($"Role IS NULL")
    .OrWhereFilter($"Age >= {minAge}").WithFilter($"Age < {maxAge}")
    .OrWhere($"UserTypeId = {userTypeId}");
```

The generated SQL will be:

```sql
SELECT FirstName, LastName, Age, Role
FROM User
WHERE (Role IN @p0 OR Role IS NULL) OR (Age >= @p1 AND Age < @p2) OR UserTypeId = @p3
```
