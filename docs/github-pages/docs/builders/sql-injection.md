# How does the library prevent SQL injection?

We are all aware of the dangers of SQL injection, however, if you are not, I suggest you read up on it [here](https://owasp.org/www-community/attacks/SQL_Injection).

**So how does the library prevent this?**

The library mitigates this by forcing you to write all your SQL queries using [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated), this is to ensure that values passed into the interpolated string are captured and parametrized. Due to this constraint, the code below won't compile.

```csharp
// Scenario 1: Won't compile
var builder = SimpleBuilder.Create("SELECT * FROM User");

// Scenario 2: Won't compile
var sql = "SELECT * FROM User";
builder = SimpleBuilder.Create(sql);

// Scenario 3: Won't compile
builder = SimpleBuilder.Create(sql + " WHERE ROLE IS NOT NULL");

// Scenario 4: Won't compile
sql = $"SELECT * FROM User WHERE UserTypeId = {userTypeId}";
builder = SimpleBuilder.Create(sql);

// Scenario 5: Won't compile
builder = SimpleBuilder.Create(sql + $" AND Role = {role}");
```
