using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using Respawn;
using Testcontainers.MsSql;

#if NET462

using System.Data.SqlClient;

#else
using Microsoft.Data.SqlClient;
#endif

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

public class MSSqlTestsFixture : IAsyncLifetime
{
    private readonly MsSqlContainer container;

    private DbConnection dbConnection = null!;

#if NET462
    private Checkpoint respawner = null!;
#else
    private Respawner respawner = null!;
#endif

    public MSSqlTestsFixture()
    {
        SeedProductTypes = new Fixture().CreateMany<ProductType>(2).ToArray();
        container = CreateSqlServerContainer();
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
        => new SqlConnection(container.GetConnectionString());

    public async Task ResetDatabaseAsync()
    {
#if NET462
        await respawner.Reset(dbConnection);
#else
        await respawner.ResetAsync(dbConnection);
#endif
    }

    private static MsSqlContainer CreateSqlServerContainer()
    {
        return new MsSqlBuilder()
            .WithPortBinding(MsSqlBuilder.MsSqlPort, true)
            .WithName("mssql")
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
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

           CREATE SEQUENCE {sequenceName:raw} START WITH 1 INCREMENT BY 1;

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} INT PRIMARY KEY DEFAULT(NEXT VALUE FOR {sequenceName:raw}),
                {nameof(Product.GlobalId):raw} UNIQUEIDENTIFIER UNIQUE NOT NULL,
                {nameof(Product.TypeId):raw} INT NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );
           """);

        await dbConnection.ExecuteAsync(builder.Sql, builder.Parameters);

        builder.Reset();
        builder.AppendIntact($"""
           CREATE PROCEDURE {StoredProcName:raw} @TypeId INT, @ProductId INT OUT
           AS
           BEGIN
                SELECT @ProductId = NEXT VALUE FOR {sequenceName:raw};
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES (@ProductId, NEWID(), @TypeId, 'procedure', GETDATE());
                RETURN @@ROWCOUNT;
           END
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
#if NET462
        respawner = new Checkpoint
        {
            SchemasToInclude = ["dbo"],
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = [nameof(ProductType)]
        };

        return Task.CompletedTask;
#else
        return CreateAsync();

        async Task CreateAsync()
        {
            respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
            {
                SchemasToInclude = ["dbo"],
                DbAdapter = DbAdapter.SqlServer,
                TablesToIgnore = [new Respawn.Graph.Table(nameof(ProductType))]
            });
        }
#endif
    }
}
