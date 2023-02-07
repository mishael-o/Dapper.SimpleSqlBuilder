using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using DotNet.Testcontainers.Configurations;
using Respawn;

#if NET461

using System.Data.SqlClient;

#else
using Microsoft.Data.SqlClient;
#endif

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

public class MSSqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "sa";
    private const string DbName = "test-db";
    private const int Port = 1433;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    private DbConnection dbConnection = null!;

#if NET461
    private Checkpoint respawner = null!;
#else
    private Respawner respawner = null!;
#endif

    public MSSqlTestsFixture()
    {
        const string dbPassword = "Mssql!Pa55w0rD";
        SeedProductTypes = new Fixture().CreateMany<ProductType>(2).ToArray();

        connectionString = $"Data Source=localhost,{Port};Initial Catalog={DbName};User ID={DbUser};Password={dbPassword};TrustServerCertificate=True";
        container = CreateSqlServerContainer(dbPassword);
    }

    public string StoredProcName { get; } = "CreateProduct";

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
        => new SqlConnection(connectionString);

    public async Task ResetDatabaseAsync()
    {
#if NET461
        await respawner.Reset(dbConnection);
#else
        await respawner.ResetAsync(dbConnection);
#endif
    }

    private static TestcontainersContainer CreateSqlServerContainer(string dbPassword)
    {
        return new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration("mcr.microsoft.com/mssql/server:2019-latest")
                {
                    Password = dbPassword,
                    Database = DbName,
                    Port = Port
                })
                .WithName("mssql")
                .Build();
    }

    private async Task CreateSchemaAsync()
    {
        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(ProductType):raw}
           (
                {nameof(ProductType.Id):raw} UNIQUEIDENTIFIER PRIMARY KEY,
                {nameof(ProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[0].Id}, {SeedProductTypes[0].Description});

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({SeedProductTypes[1].Id}, {SeedProductTypes[1].Description});

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} UNIQUEIDENTIFIER PRIMARY KEY,
                {nameof(Product.TypeId):raw} UNIQUEIDENTIFIER NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );");

        await dbConnection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE PROCEDURE {StoredProcName:raw} @TypeId UNIQUEIDENTIFIER, @UserId UNIQUEIDENTIFIER OUT
           AS
           BEGIN
                SET @UserId = NEWID();
                INSERT INTO {nameof(Product):raw}
                VALUES (@UserId, @TypeId, 'procedure', GETDATE());
                RETURN @@ROWCOUNT;
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
            SchemasToInclude = new[] { "dbo" },
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = new[] { nameof(ProductType) }
        });
#else
        respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            SchemasToInclude = new[] { "dbo" },
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = new[] { new Respawn.Graph.Table(nameof(ProductType)) }
        });
#endif
    }
}
