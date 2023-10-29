using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql;

public class PostgreSqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "dbUser";
    private const string DbName = "test-db";
    private const int Port = 5432;

    private readonly string connectionString;
    private readonly PostgreSqlContainer container;

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

    private static PostgreSqlContainer CreatePostgreSqlContainer(string dbPassword)
    {
        return new PostgreSqlBuilder()
            .WithDatabase(DbName)
            .WithUsername(DbUser)
            .WithPassword(dbPassword)
            .WithPortBinding(Port)
            .WithName("postgresql")
            .WithImage("postgres:16")
            .Build();
    }

    private async Task CreateSchemaAsync()
    {
        const string sequenceName = $"{nameof(Product)}_Id_Seq";

        var builder = SimpleBuilder.Create($"""
           CREATE TABLE {nameof(ProductType):raw}
           (
                {nameof(ProductType.Id):raw} INT PRIMARY KEY,
                {nameof(ProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[0].Id}, {SeedProductTypes[0].Description});

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[1].Id}, {SeedProductTypes[1].Description});

           CREATE SEQUENCE {sequenceName:raw} INCREMENT 1 START 1;

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} INT PRIMARY KEY DEFAULT NEXTVAL('{sequenceName:raw}'),
                {nameof(Product.GlobalId):raw} UUID UNIQUE NOT NULL,
                {nameof(Product.TypeId):raw} INT NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );
           """);

        await dbConnection.ExecuteAsync(builder.Sql, builder.Parameters);

        builder.Reset();
        builder.AppendIntact($"""
           CREATE PROCEDURE {StoredProcName:raw} (TypeId INT, OUT ProductId INT, OUT Result INT)
           AS $$
           BEGIN
                ProductId = NEXTVAL('{sequenceName:raw}');
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES (ProductId, GEN_RANDOM_UUID(), TypeId, 'procedure', NOW());
                GET DIAGNOSTICS Result = ROW_COUNT;
           END; $$
           LANGUAGE plpgsql;
           """);

        await dbConnection.ExecuteAsync(builder.Sql);
    }

    private async Task InitialiseDbConnectionAsync()
    {
        dbConnection = CreateDbConnection();
        await dbConnection.OpenAsync();
    }

    private Task InitialiseRespawnerAsync()
    {
#if NET461
        respawner = new Checkpoint
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new[] { nameof(ProductType).ToLowerInvariant() }
        };

        return Task.CompletedTask;
#else

        return CreateAsync();

        async Task CreateAsync()
        {
            respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
            {
                SchemasToInclude = new[] { "public" },
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new[] { new Respawn.Graph.Table(nameof(ProductType).ToLowerInvariant()) }
            });
        }
#endif
    }
}
