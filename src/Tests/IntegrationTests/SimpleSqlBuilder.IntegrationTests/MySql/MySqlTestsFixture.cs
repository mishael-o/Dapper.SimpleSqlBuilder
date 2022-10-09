using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public class MySqlTestsFixture : IAsyncLifetime
{
    private const string DbName = "test-db";
    private const string DbUser = "dbUser";
    private const int Port = 3306;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    public MySqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        ProductTypeInDB = fixture.Create<CustomProductType>();

        connectionString = $"Server=localhost;Port={Port};Uid={DbUser};Pwd={dbPassword};Database={DbName}";
        container = CreateMySQLContainer(dbPassword);
    }

    public string StoredProcName { get; } = "CreateProduct";

    public CustomProductType ProductTypeInDB { get; }

    public async Task InitializeAsync()
    {
        SqlMapper.AddTypeHandler(new MySqlCustomIdTypeHandler());

        await container.StartAsync();
        await CreateSchemaAsync();
    }

    public async Task DisposeAsync()
    {
        await container.DisposeAsync();
    }

    public DbConnection CreateDbConnection()
        => new MySqlConnection(connectionString);

    private static TestcontainersContainer CreateMySQLContainer(string dbPassword)
    {
        return new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mysql:8")
                .WithName("mysql")
                .WithPortBinding(Port)
                .WithEnvironment("MYSQL_ROOT_PASSWORD", Guid.NewGuid().ToString())
                .WithEnvironment("MYSQL_DATABASE", DbName)
                .WithEnvironment("MYSQL_USER", DbUser)
                .WithEnvironment("MYSQL_PASSWORD", dbPassword)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(Port))
                .Build();
    }

    private async Task CreateSchemaAsync()
    {
        using var connection = CreateDbConnection();
        await connection.OpenAsync();

        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(CustomProductType):raw}
           (
                {nameof(CustomProductType.Id):raw} BINARY(16) PRIMARY KEY,
                {nameof(CustomProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(CustomProductType):raw}
           VALUES ({ProductTypeInDB.Id}, {ProductTypeInDB.Description});

           CREATE TABLE {nameof(CustomProduct):raw}
           (
                {nameof(CustomProduct.Id):raw} BINARY(16) PRIMARY KEY,
                {nameof(CustomProduct.TypeId):raw} BINARY(16) NOT NULL REFERENCES {nameof(CustomProductType):raw}({nameof(CustomProductType.Id):raw}),
                {nameof(CustomProduct.Tag):raw} VARCHAR(50),
                {nameof(CustomProduct.CreatedDate):raw} DATE
           );");

        await connection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE PROCEDURE {StoredProcName:raw} (TypeId BINARY(16), OUT UserId BINARY(16), OUT Result INT)
           BEGIN
                SET UserId = UUID_TO_BIN(UUID());
                INSERT INTO {nameof(CustomProduct):raw}
                VALUES (UserId, TypeId, NULL, CURRENT_DATE());
                SET Result = ROW_COUNT();
           END");

        await connection.ExecuteAsync(storedProcBuilder.Sql);
    }
}
