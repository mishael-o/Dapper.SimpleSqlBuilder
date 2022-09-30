using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Microsoft.Data.SqlClient;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

public class MSSqlTestsFixture : IAsyncLifetime
{
    private readonly string connectionString;
    private readonly TestcontainersContainer container;

    public MSSqlTestsFixture()
    {
        var fixture = new Fixture();
        var dbPassword = fixture.Create<string>();
        ProductTypeInDB = fixture.Create<ProductType>();

        const int hostPortNumber = 2433;
        connectionString = $"Data Source=localhost,{hostPortNumber};Initial Catalog=master;User ID=sa;Password={dbPassword};TrustServerCertificate=True";
        container = CreateSqlServerContainer(dbPassword, hostPortNumber);
    }

    public string StoredProcName { get; } = "CreateProduct";

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
        => new SqlConnection(connectionString);

    private static TestcontainersContainer CreateSqlServerContainer(string dbPassword, int hostPortNumber)
    {
        const int containerPort = 1433;

        return new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithName("mssql")
                .WithPortBinding(hostPortNumber, containerPort)
                .WithEnvironment("MSSQL-PID", "Express")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SA_PASSWORD", dbPassword)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(containerPort))
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
           VALUES ({ProductTypeInDB.Id}, {ProductTypeInDB.Description});

           CREATE TABLE {nameof(Product):raw}
           (
                {nameof(Product.Id):raw} UNIQUEIDENTIFIER PRIMARY KEY,
                {nameof(Product.TypeId):raw} UNIQUEIDENTIFIER NOT NULL REFERENCES {nameof(ProductType):raw}({nameof(ProductType.Id):raw}),
                {nameof(Product.Tag):raw} VARCHAR(50),
                {nameof(Product.CreatedDate):raw} DATE
           );");

        using var connection = CreateDbConnection();
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