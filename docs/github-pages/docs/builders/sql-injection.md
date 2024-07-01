# How does the library prevent SQL injection?

We are all aware of the dangers of SQL injection. If you are not, you should read up on it [here](https://owasp.org/www-community/attacks/SQL_Injection).

**So how does the library prevent this?**

The library mitigates this risk by requiring you to write all your SQL queries using [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated). This approach ensures that values passed into the interpolated string are captured and parameterized. Due to this constraint, the code examples below won't compile.

```csharp
// Scenario 1: Won't compile
var builder = SimpleBuilder.Create("SELECT * FROM User");

// Scenario 2: Won't compile
var sql = "SELECT * FROM User";
builder = SimpleBuilder.Create(sql);

// Scenario 3: Won't compile
builder = SimpleBuilder.Create(sql + " WHERE Role IS NOT NULL");

// Scenario 4: Won't compile
sql = $"SELECT * FROM User WHERE UserTypeId = {userTypeId}";
builder = SimpleBuilder.Create(sql);

// Scenario 5: Won't compile
builder = SimpleBuilder.Create(sql + $" AND Role = {role}");
```
