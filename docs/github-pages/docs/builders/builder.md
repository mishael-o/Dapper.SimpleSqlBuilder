# Builder

The [`Builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) is a versatile tool for constructing static, dynamic, complex SQL queries, and stored procedures.

The `Create` method on the [`SimpleSqlBuilder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.SimpleBuilder.yml) or [`ISimpleBuilder`](../../api-docs/di/Dapper.SimpleSqlBuilder.DependencyInjection.ISimpleBuilder.yml) (when using [dependency injection](../configuration/dependency-injection.md)) creates a new [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) instance. It accepts a SQL query as one of its parameters and returns a new [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) instance.

The SQL query can be a static string or an interpolated string. The [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) parses the SQL query and extracts the parameters from it. The parameters can be accessed via the [`Parameters`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Parameters) property, and the generated SQL query can be accessed via the [`Sql`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Sql) property.

## Static SQL

```csharp
using Dapper.SimpleSqlBuilder;

int userTypeId = 10;
int age = 25;

var builder = SimpleBuilder.Create($@"
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND Age >= {age}");
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND Age >= @p1
```

For newer versions of C# (11 and later), you can also use [raw string literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string) with [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) to build your SQL queries, instead of [verbatim string literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim). See the example below.

```csharp
var builder = SimpleBuilder.Create($"""
SELECT * FROM User
WHERE UserTypeId = {userTypeId} AND Age >= {age}
""");
```

## Dynamic SQL

### Interpolated String Concatenation

You can concatenate multiple interpolated strings to build your dynamic SQL.

```csharp
var users = GetUsers(4, null, 25);

IEnumerable<User> GetUsers(int userTypeId, string role, int? minAge)
{
    var builder = SimpleBuilder.Create($"SELECT * FROM User");
    builder += $" WHERE UserTypeId = {userTypeId}";

    if (role is not null)
    {
        builder += $" AND Role = {role}";
    }

    if (minAge.HasValue)
    {
        builder += $" AND Age >= {minAge}";
    }

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT * FROM User WHERE UserTypeId = @p0 AND Age >= @p1
```

### Builder Chaining

If you prefer an alternative to interpolated string concatenation, you can use the [`Append`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Append_Dapper_SimpleSqlBuilder_AppendInterpolatedStringHandler__), [`AppendIntact`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_AppendIntact_Dapper_SimpleSqlBuilder_AppendIntactInterpolatedStringHandler__), and [`AppendNewLine`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_AppendNewLine_Dapper_SimpleSqlBuilder_AppendNewLineInterpolatedStringHandler__) methods, which can be chained.

```csharp
int userTypeId = 4;
string role = "User";

var builder = SimpleBuilder.Create($"SELECT * FROM User")
    .AppendNewLine($"WHERE UserTypeId = {userTypeId}")
    .Append($"OR Role = {role}")
    .AppendNewLine($"ORDER BY FirstName ASC");
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 OR Role = @p1
ORDER BY FirstName ASC
```

You can also use it with conditional statements. The `Append`, `AppendIntact`, and `AppendNewLine` methods all have conditional overloads. This is useful when you want to append a statement only if a condition is met.

```csharp
var users = GetUsers(4, "Admin", null, true);

IEnumerable<User> GetUsers(int userTypeId, string role, int? minAge, bool orderByName)
{
    var builder = SimpleBuilder.Create()
        .AppendIntact($"SELECT * FROM User")
        .AppendNewLine($"WHERE UserTypeId = {userTypeId}")
        .Append(role is not null, $"AND Role = {role}")
        .Append(minAge.HasValue, $"AND Age >= {minAge}")
        .AppendNewLine(orderByName, $"ORDER BY FirstName, LastName");

    return dbConnection.Query<User>(builder.Sql, builder.Parameters);
}
```

The generated SQL will be:

```sql
SELECT * FROM User
WHERE UserTypeId = @p0 AND Role = @p1
ORDER BY FirstName, LastName
```

> [!NOTE]
> The `Append` method adds a space before the SQL text by default. You can use the `AppendIntact` method if you do not want this behaviour.

## INSERT, UPDATE, and DELETE Statements

### Insert

You can perform INSERT operations with the [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) as seen in the example below.

```csharp
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($@"
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES ({user.FirstName}, {user.LastName}, {user.UserTypeId})");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```

The generated SQL will be:

```sql
INSERT INTO User (FirstName, LastName, UserTypeId)
VALUES (@p0, @p1, @p2)
```

### Update

You can perform UPDATE operations with the [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) as seen in the example below.

```csharp
int id = 1;
string role = "Admin";

var builder = SimpleBuilder.Create($@"
UPDATE User 
SET Role = {role}
WHERE Id = {id}");
```

The generated SQL will be:

```sql
UPDATE User
SET Role = @p0
WHERE Id = @p1
```

### Delete

You can perform DELETE operations with the builder as seen in the example below.

```csharp
int id = 1;
var builder = SimpleBuilder.Create($"DELETE FROM User WHERE Id = {id}");
```

The generated SQL will be:

```sql
DELETE FROM User WHERE Id = @p0
```

## Stored Procedures

You can execute stored procedures with the [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) as seen in the example below.

```csharp
var user = new User { FirstName = "John", LastName = "Doe", UserTypeId = 4 };

var builder = SimpleBuilder.Create($"CreateUserProc")
    .AddParameter("FirstName", user.FirstName)
    .AddParameter("LastName", user.LastName)
    .AddParameter("UserTypeId", user.UserTypeId)
    .AddParameter("Id", dbType: DbType.Int32, direction: ParameterDirection.Output)
    .AddParameter("Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

// Execute the stored procedure with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

// Get the output and return values
int id = builder.GetValue<int>("Id");
int result = builder.GetValue<int>("Result");
```

## Builder Reset

There are scenarios where you may want to reuse the [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) without creating a new instance. This can be achieved by calling the [`Reset`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml#Dapper_SimpleSqlBuilder_Builder_Reset) method on the [`builder`](../../api-docs/netcore/Dapper.SimpleSqlBuilder.Builder.yml) instance as seen in the example below.

```csharp
int id = 1;
var builder = SimpleBuilder.Create($"SELECT * FROM User WHERE Id = {id}");

// Execute the query with Dapper
var user = dbConnection.QuerySingle<User>(builder.Sql, builder.Parameters);

// Reset the builder
builder.Reset();

// Reuse the builder
builder.AppendIntact($"DELETE FROM User WHERE Id = {id}");

// Execute the query with Dapper
dbConnection.Execute(builder.Sql, builder.Parameters);
```
