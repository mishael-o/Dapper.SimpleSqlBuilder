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
    public async Task CreateTable_ValidateTableExists()
    {
        //Arrange
        const string tableName = "mytable";

        var builder = SimpleBuilder.Create($@"
            CREATE TABLE {tableName:raw}
            (
                Id uuid PRIMARY KEY,
                Description VARCHAR(50)
            );

            SELECT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName})");

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        var result = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task InsertDataInTable_ValidateInsert()
    {
        //Arrange
        const string tag = "insert";
        var products = Helpers.GetBaseProductComposer(postgreSqlTestsFixture.DefaultProductType.Id, tag: tag)
            .CreateMany()
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        for (var i = 0; i < products.Length; i++)
        {
            builder.AppendNewLine($@"
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({products[i].Id}, {products[i].TypeId.DefineParam(DbType.Guid)}, {products[i].Tag}, {products[i].CreatedDate});");
        }

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        //Assert
        result.Should().Be(products.Length);
    }

    [Fact]
    public async Task GetDataInTable_ValidateSelect()
    {
        //Arrange
        const string tag = "select";

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await Helpers.GenerateSeedProductsDataAsync(
            postgreSqlTestsFixture.DefaultProductType.Id,
            connection,
            tag: tag,
            productDescription: postgreSqlTestsFixture.DefaultProductType.Description);

        FormattableString subQuery = $@"
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}";

        var builder = SimpleBuilder.Create($@"
            SELECT x.*, ({subQuery}) AS {nameof(Product.Description):raw}
            FROM {nameof(Product):raw} x
            WHERE {nameof(Product.Tag):raw} = {tag}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task UpdateDataInTable_ValidateUpdate()
    {
        //Arrange
        const int count = 3;
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await Helpers.GenerateSeedProductsDataAsync(postgreSqlTestsFixture.DefaultProductType.Id, connection, count, tag);

        var builder = SimpleBuilder
            .Create($"UPDATE {nameof(Product):raw}")
            .AppendNewLine($"SET {nameof(Product.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var getUpdatedDateBuilder = SimpleBuilder
            .Create($"SELECT {nameof(Product.CreatedDate):raw} FROM {nameof(Product):raw}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        var expectedCreatedDates = await connection.QueryAsync<DateTime>(getUpdatedDateBuilder.Sql, getUpdatedDateBuilder.Parameters);

        //Assert
        result.Should().Be(count);
        expectedCreatedDates.Should().AllBeEquivalentTo(createdDate);
    }

    [Fact]
    public async Task DeleteDataInTable_ValidateDelete()
    {
        //Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await Helpers.GenerateSeedProductsDataAsync(postgreSqlTestsFixture.DefaultProductType.Id, connection, count, tag);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(Product):raw}")
            .Append($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.Create($@"
            SELECT EXISTS (SELECT 1 FROM {nameof(Product):raw} WHERE {nameof(Product.Tag):raw} = {tag})");

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        var dataExists = await connection.ExecuteScalarAsync<bool>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);

        //Assert
        result.Should().Be(count);
        dataExists.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteStoredProcedure_ValidateResult()
    {
        //Arrange
        const string resultParamName = "Result";
        const string userIdParamName = "UserId";

        var builder = SimpleBuilder.Create($"CALL {postgreSqlTestsFixture.StoredProcName:raw}(@{nameof(Product.TypeId):raw}, NULL, NULL)")
            .AddParameter(nameof(Product.TypeId), postgreSqlTestsFixture.DefaultProductType.Id, dbType: DbType.Guid)
            .AddParameter(userIdParamName, dbType: DbType.Guid, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

        using var connection = postgreSqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        //Assert
        builder.GetValue<Guid>(userIdParamName).Should().NotBe(default(Guid));
        builder.GetValue<int>(resultParamName).Should().Be(1);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => postgreSqlTestsFixture.ResetDatabaseAsync();
}