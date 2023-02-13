using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using DotNet.Testcontainers.Configurations;
using MySql.Data.MySqlClient;
using Respawn;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public class MySqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "dbUser";
    private const string DbName = "test-db";
    private const int Port = 3306;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    private DbConnection dbConnection = null!;

#if NET461
    private Checkpoint respawner = null!;
#else
    private Respawner respawner = null!;
#endif

    public MySqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        SeedProductTypes = fixture.CreateMany<CustomProductType>(2).ToArray();

        connectionString = $"Server=localhost;Port={Port};Uid={DbUser};Pwd={dbPassword};Database={DbName}";
        container = CreateMySqlContainer(dbPassword);
    }

    public string StoredProcName { get; } = "CreateProduct";

    public IReadOnlyList<CustomProductType> SeedProductTypes { get; }

    public async Task InitializeAsync()
    {
        SqlMapper.AddTypeHandler(new CustomIdTypeHandler());

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
        => new MySqlConnection(connectionString);

    public async Task ResetDatabaseAsync()
    {
#if NET461
        await respawner.Reset(dbConnection);
#else
        await respawner.ResetAsync(dbConnection);
#endif
    }

    private static TestcontainersContainer CreateMySqlContainer(string dbPassword)
    {
        return new TestcontainersBuilder<MySqlTestcontainer>()
#pragma warning disable CA2000 // Dispose objects before losing scope
                .WithDatabase(new MySqlTestcontainerConfiguration("mysql:8")
#pragma warning restore CA2000 // Dispose objects before losing scope
                {
                    Database = DbName,
                    Username = DbUser,
                    Password = dbPassword,
                    Port = Port
                })
                .WithName("mysql")
                .Build();
    }

    private async Task CreateSchemaAsync()
    {
        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(CustomProductType):raw}
           (
                {nameof(CustomProductType.Id):raw} BINARY(16) PRIMARY KEY,
                {nameof(CustomProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(CustomProductType):raw}
           VALUES ({SeedProductTypes[0].Id}, {SeedProductTypes[0].Description});

           INSERT INTO {nameof(CustomProductType):raw}
           VALUES ({SeedProductTypes[1].Id}, {SeedProductTypes[1].Description});

           CREATE TABLE {nameof(CustomProduct):raw}
           (
                {nameof(CustomProduct.Id):raw} BINARY(16) PRIMARY KEY,
                {nameof(CustomProduct.TypeId):raw} BINARY(16) NULL,
                {nameof(CustomProduct.Tag):raw} VARCHAR(50),
                {nameof(CustomProduct.CreatedDate):raw} DATE,
                FOREIGN KEY ({nameof(CustomProduct.TypeId):raw}) REFERENCES {nameof(CustomProductType):raw}({nameof(CustomProductType.Id):raw})
           );");

        await dbConnection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE PROCEDURE {StoredProcName:raw} (TypeId BINARY(16), OUT UserId BINARY(16), OUT Result INT)
           BEGIN
                SET UserId = UUID_TO_BIN(UUID());
                INSERT INTO {nameof(CustomProduct):raw}
                VALUES (UserId, TypeId, 'procedure', CURRENT_DATE());
                SET Result = ROW_COUNT();
           END");

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
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = new[] { nameof(CustomProductType) }
        });
#else
        respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = new[] { new Respawn.Graph.Table(nameof(CustomProductType)) }
        });
#endif
    }
}
