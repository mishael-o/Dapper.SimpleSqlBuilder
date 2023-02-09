using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using DotNet.Testcontainers.Configurations;
using Npgsql;
using Respawn;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql;

public class PostgreSqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "dbUser";
    private const string DbName = "test-db";
    private const int Port = 5432;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    private DbConnection dbConnection = null!;

#if NET461
    private Checkpoint respawner = null!;
#else
    private Respawner respawner = null!;
#endif

    public PostgreSqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        SeedProductTypes = fixture.CreateMany<ProductType>(2).ToArray();

        connectionString = $"Host=localhost;Port={Port};Username={DbUser};Password={dbPassword};Database={DbName}";
        container = CreatePostgreSqlContainer(dbPassword);
    }

    public string StoredProcName { get; } = "createProduct";

    public IReadOnlyList<ProductType> SeedProductTypes { get; }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        await InitialiseDbConnectionAsync();
        await CreateSchemaAsync();
        await InitialiseRespawnerAsync();
    }

    public async Task DisposeAsync()
    {
        dbConnection.Dispose();
        await container.DisposeAsync();
    }

    public DbConnection CreateDbConnection()
        => new NpgsqlConnection(connectionString);

    public async Task ResetDatabaseAsync()
    {
#if NET461
        await respawner.Reset(dbConnection);
#else
        await respawner.ResetAsync(dbConnection);
#endif
    }

    private static TestcontainersContainer CreatePostgreSqlContainer(string dbPassword)
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
#pragma warning disable CA2000 // Dispose objects before losing scope
                .WithDatabase(new PostgreSqlTestcontainerConfiguration("postgres:14")
#pragma warning restore CA2000 // Dispose objects before losing scope
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
        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(ProductType):raw}
           (
                {nameof(ProductType.Id):raw} uuid PRIMARY KEY,
                {nameof(ProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[0].Id}, {SeedProductTypes[0].Description});

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[1].Id}, {SeedProductTypes[1].Description});

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} uuid PRIMARY KEY,
                {nameof(Product.TypeId):raw} uuid NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );");

        await dbConnection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

           CREATE PROCEDURE {StoredProcName:raw} (TypeId uuid, out UserId uuid, out Result int)
           AS $$
           BEGIN
                UserId = gen_random_uuid();
                INSERT INTO {nameof(Product):raw}
                VALUES (UserId, TypeId, 'procedure', now());
                GET DIAGNOSTICS Result = ROW_COUNT;
           END; $$
           LANGUAGE plpgsql;");

        await dbConnection.ExecuteAsync(storedProcBuilder.Sql);
    }

    private async Task InitialiseDbConnectionAsync()
    {
        dbConnection = CreateDbConnection();
        await dbConnection.OpenAsync();
    }

    private async Task InitialiseRespawnerAsync()
    {
#if NET461
        await Task.Run(() => respawner = new Checkpoint
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new[] { nameof(ProductType).ToLowerInvariant() }
        });
#else
        respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new[] { new Respawn.Graph.Table(nameof(ProductType).ToLowerInvariant()) }
        });
#endif
    }
}
