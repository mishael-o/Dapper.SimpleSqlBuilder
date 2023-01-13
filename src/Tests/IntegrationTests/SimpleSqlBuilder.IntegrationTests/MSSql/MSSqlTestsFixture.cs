using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using DotNet.Testcontainers.Configurations;

#if NET461

using System.Data.SqlClient;

#else
using Microsoft.Data.SqlClient;
#endif

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

public class MSSqlTestsFixture : IAsyncLifetime
{
    private const string DbUser = "sa";
    private const string DbName = "tempdb";
    private const int Port = 1433;

    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    public MSSqlTestsFixture()
    {
        const string dbPassword = "Mssql!Pa55w0rD";
        ProductTypeInDB = new Fixture().Create<ProductType>();

        connectionString = $"Data Source=localhost,{Port};Initial Catalog={DbName};User ID={DbUser};Password={dbPassword};TrustServerCertificate=True";
        container = CreateSqlServerContainer(dbPassword);
    }

    public string StoredProcName { get; } = "CreateProduct";

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
        => new SqlConnection(connectionString);

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
        using var connection = CreateDbConnection();
        await connection.OpenAsync();

        var tableBuilder = SimpleBuilder.Create($@"
           CREATE TABLE {nameof(ProductType):raw}
           (
                {nameof(ProductType.Id):raw} UNIQUEIDENTIFIER PRIMARY KEY,
                {nameof(ProductType.Description):raw} VARCHAR(255)
           );

           INSERT INTO {nameof(ProductType):raw}
           VALUES ({ProductTypeInDB.Id}, {ProductTypeInDB.Description});

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} UNIQUEIDENTIFIER PRIMARY KEY,
                {nameof(Product.TypeId):raw} UNIQUEIDENTIFIER NOT NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );");

        await connection.ExecuteAsync(tableBuilder.Sql, tableBuilder.Parameters);

        var storedProcBuilder = SimpleBuilder.Create($@"
           CREATE PROCEDURE {StoredProcName:raw} @TypeId UNIQUEIDENTIFIER, @UserId UNIQUEIDENTIFIER OUT
           AS
           BEGIN
                SET @UserId = NEWID();
                INSERT INTO {nameof(Product):raw}
                VALUES (@UserId, @TypeId, NULL, GETDATE());
                RETURN @@ROWCOUNT;
           END");

        // Stored procedure can also be created from 'Append' methods string

        //var storedProcBuilder = SimpleBuilder.Create()
        //   .AppendIntact($"CREATE PROCEDURE {StoredProcName:raw} @TypeId UNIQUEIDENTIFIER, @UserId UNIQUEIDENTIFIER OUT")
        //   .AppendNewLine($"AS")
        //   .AppendNewLine($"BEGIN")
        //   .AppendNewLine($"SET @UserId = NEWID();")
        //   .AppendNewLine($"INSERT INTO {nameof(Product):raw}")
        //   .AppendNewLine($"VALUES (@UserId, @TypeId, NULL, GETDATE());")
        //   .AppendNewLine($"RETURN @@ROWCOUNT;")
        //   .AppendNewLine($"END");

        await connection.ExecuteAsync(storedProcBuilder.Sql);
    }
}
