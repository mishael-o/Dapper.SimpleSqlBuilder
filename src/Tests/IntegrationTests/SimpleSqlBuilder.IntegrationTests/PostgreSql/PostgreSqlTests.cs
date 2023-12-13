using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.PostgreSql;

[Collection(nameof(PostgreSqlTestsCollection))]
public class PostgreSqlTests : IAsyncLifetime
{
    private readonly PostgreSqlTestsFixture postgreSqlTestsFixture;

    public PostgreSqlTests(PostgreSqlTestsFixture postgreSqlTestsFixture)
    {
        this.postgreSqlTestsFixture = postgreSqlTestsFixture;
    }

    [Fact]
    public async Task Builder_CreatesTable_ReturnsBoolean()
    {
        // Arrange
        const string tableName = "mytable";

        var builder = SimpleBuilder.Create($"""
            CREATE TABLE {tableName:raw}
            (
                Id INT PRIMARY KEY,
                Description VARCHAR(50)
            );

            SELECT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName})
            """);

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
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

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
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

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await ProductGenerator.GenerateSeedProductsAsync(connection, productTypeId: postgreSqlTestsFixture.SeedProductTypes[0].Id, tag: tag);

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

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"UPDATE {nameof(Product):raw}")
            .AppendNewLine($"SET {nameof(Product.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        builder.Reset();
        builder.AppendIntact($"""
            SELECT {nameof(Product.CreatedDate):raw} FROM {nameof(Product):raw}
            WHERE {nameof(Product.Tag):raw} = {tag}
            """);
        var expectedCreatedDates = await connection.QueryAsync<DateTime>(builder.Sql, builder.Parameters);
        expectedCreatedDates.Should().AllBeEquivalentTo(createdDate);
    }

    [Fact]
    public async Task Builder_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(Product):raw}")
            .Append($"WHERE {nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        builder.Reset();
        builder.AppendIntact($"SELECT EXISTS (SELECT 1 FROM {nameof(Product):raw} WHERE {nameof(Product.Tag):raw} = {tag})");
        var dataExists = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);
        dataExists.Should().BeFalse();
    }

    [Fact]
    public async Task Builder_ExecutesStoredProcedure_ReturnsTask()
    {
        // Arrange
        const string resultParamName = "Result";
        const string productIdParamName = "ProductId";

        var builder = SimpleBuilder.Create($"CALL {postgreSqlTestsFixture.StoredProcName:raw}(@{nameof(Product.TypeId):raw}, NULL, NULL)")
            .AddParameter(nameof(Product.TypeId), postgreSqlTestsFixture.SeedProductTypes[0].Id, dbType: DbType.Int32)
            .AddParameter(productIdParamName, dbType: DbType.Int32, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        // Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        builder.GetValue<int>(productIdParamName).Should().NotBe(default);
        builder.GetValue<int>(resultParamName).Should().Be(1);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => postgreSqlTestsFixture.ResetDatabaseAsync();
}
