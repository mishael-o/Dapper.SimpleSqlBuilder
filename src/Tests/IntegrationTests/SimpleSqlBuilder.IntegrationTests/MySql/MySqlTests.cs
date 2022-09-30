using System.Data;
using System.Data.Common;
using Dapper.SimpleSqlBuilder.Extensions;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public class MySqlTests : IClassFixture<MySqlTestsFixture>
{
    private readonly MySqlTestsFixture mySqlTestsFixture;

    public MySqlTests(MySqlTestsFixture mySqlTestsFixture)
    {
        this.mySqlTestsFixture = mySqlTestsFixture;
    }

    [Fact]
    public async Task CreateTable_ValidateTableExists()
    {
        //Arrange
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

        //Act
        var result = await connection.ExecuteScalarAsync<bool>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task InsertDataInTable_ValidateInsert()
    {
        //Arrange
        var products = GetBaseProductComposer()
            .CreateMany()
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.AppendNewLine($@"
                INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.CreatedDate):raw})
                VALUES ({product.Id}, {product.TypeId.DefineParam(DbType.Binary, size: 16)}, {product.CreatedDate});");
        }

        using var connection = mySqlTestsFixture.CreateDbConnection();

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
        using var connection = mySqlTestsFixture.CreateDbConnection();
        var products = await GenerateSeedDataAsync(connection);

        FormattableString subQuery = $@"
            SELECT {nameof(CustomProductType.Description):raw}
            FROM {nameof(CustomProductType):raw}
            WHERE {nameof(CustomProductType.Id):raw} = x.{nameof(CustomProduct.TypeId):raw}";

        var builder = SimpleBuilder.Create($@"
            SELECT x.*, ({subQuery}) AS {nameof(CustomProduct.Description):raw}
            FROM {nameof(CustomProduct):raw} x
            WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        //Act
        var productsInDb = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

        //Assert
        productsInDb.Should().BeEquivalentTo(products);

        async Task<CustomProduct[]> GenerateSeedDataAsync(DbConnection connection)
        {
            var products = GetBaseProductComposer()
                .With(x => x.Description, mySqlTestsFixture.ProductTypeInDB.Description)
                .With(x => x.Tag, tag)
                .CreateMany()
                .ToArray();

            var builder = SimpleBuilder.Create(reuseParameters: true);

            for (var i = 0; i < products.Length; i++)
            {
                builder.AppendNewLine(
                   $@"INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.Tag):raw}, {nameof(CustomProduct.CreatedDate):raw})
                   VALUES ({products[i].Id}, {products[i].TypeId}, {products[i].Tag}, {products[i].CreatedDate.DefineParam(DbType.Date)});");
            }

            await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

            return products;
        }
    }

    [Fact]
    public async Task UpdateDataInTable_ValidateUpdate()
    {
        //Arrange
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await GenerateSeedDataAsync(connection);

        var updateBuilder = SimpleBuilder
            .Create($"UPDATE {nameof(CustomProduct):raw}")
            .AppendNewLine($"SET {nameof(CustomProduct.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        var getUpdatedDateBuilder = SimpleBuilder
            .Create($"SELECT {nameof(CustomProduct.CreatedDate):raw} FROM {nameof(CustomProduct):raw}")
            .AppendNewLine($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        //Act
        var result = await connection.ExecuteAsync(updateBuilder.Sql, updateBuilder.Parameters);
        var expectedCreatedDate = await connection.ExecuteScalarAsync<DateTime>(getUpdatedDateBuilder.Sql, getUpdatedDateBuilder.Parameters);

        //Assert
        result.Should().Be(1);
        expectedCreatedDate.Should().Be(createdDate);

        async Task GenerateSeedDataAsync(DbConnection connection)
        {
            var product = GetBaseProductComposer()
                .With(x => x.Tag, tag)
                .Create();

            var builder = SimpleBuilder.Create($@"
                INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.Tag):raw}, {nameof(CustomProduct.CreatedDate):raw})
                VALUES ({product.Id}, {product.TypeId}, {product.Tag}, {product.CreatedDate});");

            await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        }
    }

    [Fact]
    public async Task DeleteDataInTable_ValidateDelete()
    {
        //Arrange
        const string tag = "delete";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await GenerateSeedDataAsync(connection);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(CustomProduct):raw}")
            .Append($"WHERE {nameof(CustomProduct.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.Create($@"
            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM {nameof(CustomProduct):raw} WHERE {nameof(CustomProduct.Tag):raw} = {tag}) THEN 1
                ELSE 0
            END");

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        var dataExists = await connection.ExecuteScalarAsync<bool>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);

        //Assert
        result.Should().Be(1);
        dataExists.Should().BeFalse();

        async Task GenerateSeedDataAsync(DbConnection connection)
        {
            var product = GetBaseProductComposer()
                .With(x => x.Tag, tag)
                .Create();

            var builder = SimpleBuilder.Create($@"
                INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.Tag):raw}, {nameof(CustomProduct.CreatedDate):raw})
                VALUES ({product.Id}, {product.TypeId}, {product.Tag}, {product.CreatedDate});");

            await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        }
    }

    [Fact]
    public async Task ExecuteStoredProcedure_ValidateResult()
    {
        //Arrange
        const string resultParamName = "Result";
        const string userIdParamName = "UserId";

        var builder = SimpleBuilder.Create($"{mySqlTestsFixture.StoredProcName:raw}")
            .AddParameter(nameof(CustomProduct.TypeId), mySqlTestsFixture.ProductTypeInDB.Id, dbType: DbType.Binary, size: 16)
            .AddParameter(userIdParamName, dbType: DbType.Binary, size: 16, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.Output);

        using var connection = mySqlTestsFixture.CreateDbConnection();

        //Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

        //Assert
        builder.GetValue<int>(resultParamName).Should().Be(1);
        builder.GetValue<byte[]>(userIdParamName).Should().NotBeNullOrEmpty();
    }

    private AutoFixture.Dsl.IPostprocessComposer<CustomProduct> GetBaseProductComposer()
    {
        return new Fixture()
            .Build<CustomProduct>()
            .With(x => x.TypeId, mySqlTestsFixture.ProductTypeInDB.Id)
            .With(x => x.CreatedDate, DateTime.Now.Date);
    }
}
