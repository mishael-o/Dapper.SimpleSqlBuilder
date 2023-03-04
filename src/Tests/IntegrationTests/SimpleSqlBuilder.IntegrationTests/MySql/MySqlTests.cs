using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

[Collection(nameof(MySqlTestsCollection))]
public class MySqlTests : IAsyncLifetime
{
    private readonly MySqlTestsFixture mySqlTestsFixture;

    public MySqlTests(MySqlTestsFixture mySqlTestsFixture)
    {
        this.mySqlTestsFixture = mySqlTestsFixture;
    }

    [Fact]
    public async Task Builder_CreatesTable_ReturnsBoolean()
    {
        // Arrange
        const string tableName = "MyTable";

        var builder = SimpleBuilder.Create($@"
            CREATE TABLE {tableName:raw}
            (
                Id BINARY(16) PRIMARY KEY,
                Description VARCHAR(50)
            );

            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName}) THEN 1
                ELSE 0
            END;");

        using var connection = mySqlTestsFixture.CreateDbConnection();
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
        var products = ProductHelpers.GetCustomProductFixture(tag: tag)
            .CreateMany()
            .AsArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        for (var i = 0; i < products.Length; i++)
        {
            builder.AppendNewLine($@"
                INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(CustomProduct.CreatedDate):raw})
                VALUES ({products[i].Id}, {products[i].TypeId},  {products[i].Tag}, {products[i].CreatedDate.DefineParam(DbType.DateTime)});");
        }

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(products.Length);
    }

    [Fact]
    public async Task Builder_GetsProductsWithSelectTag_ReturnsIEnumerableOfCustomProduct()
    {
        // Arrange
        const string tag = "select";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection,
            productTypeId: mySqlTestsFixture.SeedProductTypes[0].Id,
            tag: tag,
            productDescription: mySqlTestsFixture.SeedProductTypes[0].Description);

        FormattableString subQuery = $@"
            SELECT {nameof(CustomProductType.Description):raw}
            FROM {nameof(CustomProductType):raw}
            WHERE {nameof(CustomProductType.Id):raw} = x.{nameof(CustomProduct.TypeId):raw}";

        var builder = SimpleBuilder.Create($@"
            SELECT x.*, ({subQuery}) AS {nameof(CustomProduct.Description):raw}
            FROM {nameof(CustomProduct):raw} x
            WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        // Act
        var result = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

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

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedCustomProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"UPDATE {nameof(CustomProduct):raw}")
            .AppendNewLine($"SET {nameof(CustomProduct.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        var getUpdatedDateBuilder = SimpleBuilder
            .Create($"SELECT {nameof(CustomProduct.CreatedDate):raw} FROM {nameof(CustomProduct):raw}")
            .AppendNewLine($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

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

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedCustomProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(CustomProduct):raw}")
            .Append($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.Create($@"
            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM {nameof(CustomProduct):raw} WHERE {nameof(CustomProduct.Tag):raw} = {tag}) THEN 1
                ELSE 0
            END");

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
        const string userIdParamName = "UserId";

        var builder = SimpleBuilder.Create($"{mySqlTestsFixture.StoredProcName:raw}")
            .AddParameter(nameof(CustomProduct.TypeId), mySqlTestsFixture.SeedProductTypes[0].Id)
            .AddParameter(userIdParamName, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        // Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

        // Assert
        builder.GetValue<byte[]>(userIdParamName).Should().NotBeNullOrEmpty();
        builder.GetValue<int>(resultParamName).Should().Be(1);
    }

    public Task InitializeAsync()
    => Task.CompletedTask;

    public Task DisposeAsync()
        => mySqlTestsFixture.ResetDatabaseAsync();
}
