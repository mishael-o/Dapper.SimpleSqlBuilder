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
        await connection.OpenAsync();

        // Act
        var result = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Builder_InsertsProducts_ReturnsInteger()
    {
        // Arrange
        const string tag = "insert";
        var products = ProductHelpers.GetProductFixture(tag: tag)
            .CreateMany()
            .AsArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        for (var i = 0; i < products.Length; i++)
        {
            builder.AppendNewLine($"""
                INSERT INTO {nameof(Product):raw} ({nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({products[i].GlobalId}, {products[i].TypeId.DefineParam(DbType.Int32)}, {products[i].Tag}, {products[i].CreatedDate});
                """);
        }

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(products.Length);
    }

    [Fact]
    public async Task Builder_GetsProductsWithSelectTag_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const string tag = "select";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await ProductHelpers.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag);

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
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Builder_UpdatesProductsWithUpdateTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"UPDATE {nameof(Product):raw}")
            .AppendNewLine($"SET {nameof(Product.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var getUpdatedDateBuilder = SimpleBuilder
            .Create($"SELECT {nameof(Product.CreatedDate):raw} FROM {nameof(Product):raw}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        var expectedCreatedDates = await connection.QueryAsync<DateTime>(getUpdatedDateBuilder.Sql, getUpdatedDateBuilder.Parameters);
        expectedCreatedDates.Should().AllBeEquivalentTo(createdDate);
    }

    [Fact]
    public async Task Builder_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(Product):raw}")
            .Append($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.Create($"""
            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM {nameof(Product):raw} WHERE {nameof(Product.Tag):raw} = {tag}) THEN 1
                ELSE 0
            END
            """);

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        var dataExists = await connection.ExecuteScalarAsync<bool>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);
        dataExists.Should().BeFalse();
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
        await connection.OpenAsync();

        // Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

        // Assert
        builder.GetValue<int>(productIdParamName).Should().NotBe(default);
        builder.GetValue<int>(resultParamName).Should().Be(1);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => mssqlTestsFixture.ResetDatabaseAsync();
}
