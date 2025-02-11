﻿using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;
using MySqlConnector;
using Respawn;
using Testcontainers.MySql;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public class MySqlTestsFixture : IAsyncLifetime
{
    private readonly MySqlContainer container;

    private DbConnection dbConnection = null!;

#if NETFRAMEWORK
    private Checkpoint respawner = null!;
#else
    private Respawner respawner = null!;
#endif

    public MySqlTestsFixture()
    {
        var fixture = new Fixture();
        SeedProductTypes = fixture.CreateMany<ProductType>(2).ToArray();
        container = CreateMySqlContainer();
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
        => new MySqlConnection(container.GetConnectionString());

    public async Task ResetDatabaseAsync()
    {
#if NETFRAMEWORK
        await respawner.Reset(dbConnection);
#else
        await respawner.ResetAsync(dbConnection);
#endif
    }

    private static MySqlContainer CreateMySqlContainer()
    {
        return new MySqlBuilder()
            .WithPortBinding(MySqlBuilder.MySqlPort, true)
            .WithName("mysql")
            .WithImage("mysql:9")
            .Build();
    }

    private async Task CreateSchemaAsync()
    {
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

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} INT PRIMARY KEY AUTO_INCREMENT,
                {nameof(Product.GlobalId):raw} CHAR(36) NOT NULL,
                {nameof(Product.TypeId):raw} INT NULL,
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE,
                FOREIGN KEY ({nameof(Product.TypeId):raw}) REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                UNIQUE ({nameof(Product.GlobalId):raw})
           );
           """);

        await dbConnection.ExecuteAsync(builder.Sql, builder.Parameters);

        builder.Reset();
        builder.AppendIntact($"""
           CREATE PROCEDURE {StoredProcName:raw} (TypeId INT, OUT ProductId INT, OUT Result INT)
           BEGIN
                INSERT INTO {nameof(Product):raw} ({nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES (UUID(), TypeId, 'procedure', CURRENT_DATE());
                SET ProductId = LAST_INSERT_ID();
                SET Result = ROW_COUNT();
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
#if NETFRAMEWORK
        respawner = new Checkpoint
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = [nameof(ProductType)]
        };

        return Task.CompletedTask;
#else
        return CreateAsync();

        async Task CreateAsync()
        {
            respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.MySql,
                TablesToIgnore = [new Respawn.Graph.Table(nameof(ProductType))]
            });
        }
#endif
    }
}
