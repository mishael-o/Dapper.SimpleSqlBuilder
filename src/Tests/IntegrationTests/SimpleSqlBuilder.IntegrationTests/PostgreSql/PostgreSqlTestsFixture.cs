using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Npgsql;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql;

public class PostgreSqlTestsFixture : IAsyncLifetime
{
    private const string DbName = "test-db";
    private const string DbUser = "dbUser";

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    public PostgreSqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        ProductTypeInDB = fixture.Create<ProductType>();

        const int hostPortNumber = 6432;
        connectionString = $"Host=localhost;Port={hostPortNumber};Username={DbUser};Password={dbPassword};Database={DbName}";
        container = CreatePostgreSQLContainer(dbPassword, hostPortNumber);
    }

    public string StoredProcName { get; } = "createProduct";

    public ProductType ProductTypeInDB { get; }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        using var conn = CreateDbConnection();
        await CreateSchemaAsync();
    }

    public async Task DisposeAsync()
    {
        await container.DisposeAsync();
    }

    public DbConnection CreateDbConnection()
        => new NpgsqlConnection(connectionString);

    private static TestcontainersContainer CreatePostgreSQLContainer(string dbPassword, int hostPortNumber)
    {
        const int containerPort = 5432;

        return new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("postgres:14")
                .WithName("postgresql")
                .WithPortBinding(hostPortNumber, containerPort)
                .WithEnvironment("POSTGRES_DB", DbName)
                .WithEnvironment("POSTGRES_USER", DbUser)
                .WithEnvironment("POSTGRES_PASSWORD", dbPassword)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(containerPort))
                .Build();
    }

    private async Task CreateSchemaAsync()
    {
        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(ProductType):raw}
           (
                {nameof(ProductType.Id):raw} uuid PRIMARY KEY,
                {nameof(ProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({ProductTypeInDB.Id}, {ProductTypeInDB.Description});

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} uuid PRIMARY KEY,
                {nameof(Product.TypeId):raw} uuid NOT NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );");

        using var connection = CreateDbConnection();
        await connection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

           CREATE PROCEDURE {StoredProcName:raw} (TypeId uuid, out UserId uuid, out Result int)
           AS $$
           BEGIN
                UserId = gen_random_uuid();
                INSERT INTO {nameof(Product):raw}
                VALUES (UserId, TypeId, now());
                GET DIAGNOSTICS Result = ROW_COUNT;
           END; $$
           LANGUAGE plpgsql;");

        await connection.ExecuteAsync(storedProcBuilder.Sql);
    }
}