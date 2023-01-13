using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using DotNet.Testcontainers.Configurations;
using Npgsql;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql;

public class PostgreSqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "dbUser";
    private const string DbName = "test-db";
    private const int Port = 5432;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    public PostgreSqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        ProductTypeInDB = fixture.Create<ProductType>();

        connectionString = $"Host=localhost;Port={Port};Username={DbUser};Password={dbPassword};Database={DbName}";
        container = CreatePostgreSQLContainer(dbPassword);
    }

    public string StoredProcName { get; } = "createProduct";

    public ProductType ProductTypeInDB { get; }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        await CreateSchemaAsync();
    }

    public async Task DisposeAsync()
    {
        await container.DisposeAsync();
    }

    public DbConnection CreateDbConnection()
        => new NpgsqlConnection(connectionString);

    private static TestcontainersContainer CreatePostgreSQLContainer(string dbPassword)
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration("postgres:14")
                {
                    Database = DbName,
                    Username = DbUser,
                    Password = dbPassword,
                    Port = Port
                })
                .WithName("postgresql")
                .Build();
    }

    private async Task CreateSchemaAsync()
    {
        using var connection = CreateDbConnection();
        await connection.OpenAsync();

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
