using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

[Collection(nameof(MSSqlTestsCollection))]
public class MSSqlTests : IAsyncLifetime
{
    private readonly MSSqlTestsFixture mssqlTestsFixture;

    public MSSqlTests(MSSqlTestsFixture mssqlTestsFixture)
    {
        this.mssqlTestsFixture = mssqlTestsFixture;
    }

    [Fact]
    public async Task Builder_CreatesTable_ReturnsBoolean()
    {
        // Arrange
        const string tableName = "MyTable";

        var builder = SimpleBuilder.Create($"""
            CREATE TABLE {tableName:raw}
            (
                Id INT PRIMARY KEY,
                Description VARCHAR(50)
            );

            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName}) THEN 1
                ELSE 0
            END;
            """);

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task Builder_InsertsProducts_ReturnsInteger()
    {
        // Arrange
        const string tag = "insert";
        var products = ProductGenerator.GetProductFixture(tag: tag)
            .CreateMany()
            .AsArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        for (var i = 0; i < products.Length; i++)
        {
            builder.AppendIntact($"""
                INSERT INTO {nameof(Product):raw} ({nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({products[i].GlobalId}, {products[i].TypeId.DefineParam(DbType.Int32)}, {products[i].Tag}, {products[i].CreatedDate});
                """);
        }

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products.Length);
    }

    [Fact]
    public async Task Builder_GetsProductsWithSelectTag_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const string tag = "select";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = await ProductGenerator.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag);

        FormattableString subQuery = $"""
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}
            """;

        var builder = SimpleBuilder.Create($"""
            SELECT x.*, ({subQuery}) AS {nameof(Product.Description):raw}
            FROM {nameof(Product):raw} x
            WHERE {nameof(Product.Tag):raw} = {tag}
            """);

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products, ignoreOrder: true);
    }

    [Fact]
    public async Task Builder_UpdatesProductsWithUpdateTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "update";
        var createdDate = DateTime.UtcNow.AddDays(10);

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"UPDATE {nameof(Product):raw}")
            .AppendNewLine($"SET {nameof(Product.CreatedDate):raw} = {createdDate.DefineParam(DbType.DateTime2)}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(count);

        builder.Reset();
        builder.AppendIntact($"""
            SELECT {nameof(Product.CreatedDate):raw} FROM {nameof(Product):raw}
            WHERE {nameof(Product.Tag):raw} = {tag}
            """);
        var expectedCreatedDates = await connection.QueryAsync<DateTime>(builder.Sql, builder.Parameters);
        expectedCreatedDates.ShouldAllBe(date => date == createdDate);
    }

    [Fact]
    public async Task Builder_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(Product):raw}")
            .Append($"WHERE {nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(count);

        builder.Reset();
        builder.AppendIntact($"""
            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM {nameof(Product):raw} WHERE {nameof(Product.Tag):raw} = {tag}) THEN 1
                ELSE 0
            END
            """);
        var dataExists = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);
        dataExists.ShouldBeFalse();
    }

    [Fact]
    public async Task Builder_ExecutesStoredProcedure_ReturnsTask()
    {
        // Arrange
        const string resultParamName = "Result";
        const string productIdParamName = "ProductId";

        var builder = SimpleBuilder.Create($"{mssqlTestsFixture.StoredProcName:raw}")
            .AddParameter(nameof(Product.TypeId), mssqlTestsFixture.SeedProductTypes[0].Id, dbType: DbType.Int32)
            .AddParameter(productIdParamName, dbType: DbType.Int32, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

        // Assert
        builder.GetValue<int>(productIdParamName).ShouldNotBe(default);
        builder.GetValue<int>(resultParamName).ShouldBe(1);
    }

    public ValueTask InitializeAsync()
        => default;

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return new(mssqlTestsFixture.ResetDatabaseAsync());
    }
}
