using System.Data;
using System.Data.Common;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

public class MSSqlTests : IClassFixture<MSSqlTestsFixture>
{
    private readonly MSSqlTestsFixture mssqlTestsFixture;

    public MSSqlTests(MSSqlTestsFixture mssqlTestsFixture)
    {
        this.mssqlTestsFixture = mssqlTestsFixture;
    }

    [Fact]
    public async Task CreateTable_ValidateTableExists()
    {
        //Arrange
        const string tableName = "MyTable";

        var builder = SimpleBuilder.Create($@"
            CREATE TABLE {tableName:raw}
            (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Description VARCHAR(50)
            );

            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName}) THEN 1
                ELSE 0
            END;");

        using var connection = mssqlTestsFixture.CreateDbConnection();
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
        var products = GetBaseProductComposer()
            .CreateMany()
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.AppendNewLine($@"
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({product.Id}, {product.TypeId.DefineParam(DbType.Guid)}, {product.CreatedDate});");
        }

        using var connection = mssqlTestsFixture.CreateDbConnection();
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

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await GenerateSeedDataAsync(connection);

        FormattableString subQuery = $@"
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}";

        var builder = SimpleBuilder.Create($@"
            SELECT x.*, ({subQuery}) AS {nameof(Product.Description):raw}
            FROM {nameof(Product):raw} x
            WHERE {nameof(Product.Tag):raw} = {tag}");

        //Act
        var productsInDb = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        productsInDb.Should().BeEquivalentTo(products);

        async Task<Product[]> GenerateSeedDataAsync(DbConnection connection)
        {
            var products = GetBaseProductComposer()
                .With(x => x.Description, mssqlTestsFixture.ProductTypeInDB.Description)
                .With(x => x.Tag, tag)
                .CreateMany()
                .ToArray();

            var builder = SimpleBuilder.Create(reuseParameters: true);

            for (var i = 0; i < products.Length; i++)
            {
                builder.AppendNewLine(
                   $@"INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                   VALUES ({products[i].Id}, {products[i].TypeId}, {products[i].Tag}, {products[i].CreatedDate.DefineParam(DbType.Date)});");
            }

            await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

            return products;
        }
    }

    [Fact]
    public async Task UpdateDataInTable_ValidateUpdate()
    {
        //Arrange
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await GenerateSeedDataAsync(connection);

        var updateBuilder = SimpleBuilder
            .Create($"UPDATE {nameof(Product):raw}")
            .AppendNewLine($"SET {nameof(Product.CreatedDate):raw} = {createdDate}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var getUpdatedDateBuilder = SimpleBuilder
            .Create($"SELECT {nameof(Product.CreatedDate):raw} FROM {nameof(Product):raw}")
            .AppendNewLine($"WHERE {nameof(Product.Tag):raw} = {tag}");

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
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({product.Id}, {product.TypeId}, {product.Tag}, {product.CreatedDate});");

            await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        }
    }

    [Fact]
    public async Task DeleteDataInTable_ValidateDelete()
    {
        //Arrange
        const string tag = "delete";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await GenerateSeedDataAsync(connection);

        var builder = SimpleBuilder
            .Create($"DELETE FROM {nameof(Product):raw}")
            .Append($"WHERE {nameof(Product.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.Create($@"
            SELECT
            CASE
                WHEN EXISTS (SELECT 1 FROM {nameof(Product):raw} WHERE {nameof(Product.Tag):raw} = {tag}) THEN 1
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
                INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
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

        var builder = SimpleBuilder.Create($"{mssqlTestsFixture.StoredProcName:raw}")
            .AddParameter(nameof(Product.TypeId), mssqlTestsFixture.ProductTypeInDB.Id, dbType: DbType.Guid)
            .AddParameter(userIdParamName, dbType: DbType.Guid, direction: ParameterDirection.Output)
            .AddParameter(resultParamName, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        await connection.ExecuteAsync(builder.Sql, builder.Parameters, commandType: CommandType.StoredProcedure);

        //Assert
        builder.GetValue<int>(resultParamName).Should().Be(1);
        builder.GetValue<Guid>(userIdParamName).Should().NotBe(default(Guid));
    }

    private AutoFixture.Dsl.IPostprocessComposer<Product> GetBaseProductComposer()
    {
        return new Fixture()
            .Build<Product>()
            .With(x => x.TypeId, mssqlTestsFixture.ProductTypeInDB.Id)
            .With(x => x.CreatedDate, DateTime.Now.Date);
    }
}
