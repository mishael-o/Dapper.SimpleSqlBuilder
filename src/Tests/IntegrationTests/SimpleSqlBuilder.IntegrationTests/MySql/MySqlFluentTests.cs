using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

[Collection(nameof(MySqlTestsCollection))]
public class MySqlFluentTests : IAsyncLifetime
{
    private readonly MySqlTestsFixture mySqlTestsFixture;

    public MySqlFluentTests(MySqlTestsFixture mySqlTestsFixture)
    {
        this.mySqlTestsFixture = mySqlTestsFixture;
    }

    [Fact]
    public async Task Insert_AddsProducts_ReturnsInteger()
    {
        // Arrange
        const string tag = "insert";
        var product = ProductHelpers
            .GetCustomProductFixture(tag: tag)
            .Create();

        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"{nameof(CustomProduct):raw}")
            .Columns($"{nameof(CustomProduct.Id):raw}")
            .Columns($"{nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.Tag):raw}")
            .Columns($"{nameof(CustomProduct.CreatedDate):raw}")
            .Values($"{product.Id}")
            .Values($"{product.TypeId}")
            .Values($"{product.Tag}, {product.CreatedDate.DefineParam(DbType.DateTime)}");

        var insertCountBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(CustomProduct):raw}")
            .Where($"{nameof(CustomProduct.Tag):raw} = {tag}");

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        var insertCount = await connection.ExecuteScalarAsync<int>(insertCountBuilder.Sql, insertCountBuilder.Parameters);
        result.Should().Be(1).And.Be(insertCount);
    }

    [Fact]
    public async Task Select_GetsProductsWithSelectTag_ReturnsIEnumerableOfCustomProduct()
    {
        // Arrange
        const string tag = "select";
        const string tag2 = $"{tag}2";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection,
            productTypeId: mySqlTestsFixture.SeedProductTypes[0].Id,
            tag: tag,
            productDescription: mySqlTestsFixture.SeedProductTypes[0].Description)).ToList();

        products.AddRange(await ProductHelpers.GenerateSeedCustomProductsAsync(connection, tag: tag2));

        FormattableString subQuery = $@"
            SELECT {nameof(CustomProductType.Description):raw}
            FROM {nameof(CustomProductType):raw}
            WHERE {nameof(CustomProductType.Id):raw} = x.{nameof(CustomProduct.TypeId):raw}";

        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery}) AS {nameof(CustomProduct.Description):raw}")
            .From($"{nameof(CustomProduct):raw} x")
            .Where($"{nameof(CustomProduct.Tag):raw} = {tag}")
            .OrWhere($"{nameof(CustomProduct.Tag):raw} = {tag2}");

        // Act
        var result = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByInnerJoinOnProductAndProductType_ReturnsIEnumerableOfCustomProduct()
    {
        // Arrange
        const string tag = "selectInnerJoin";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection, productTypeId: mySqlTestsFixture.SeedProductTypes[0].Id, tag: tag, productDescription: mySqlTestsFixture.SeedProductTypes[0].Description);
        await ProductHelpers.GenerateSeedCustomProductsAsync(connection, productTypeId: mySqlTestsFixture.SeedProductTypes[1].Id, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*, pt.{nameof(CustomProductType.Description):raw}")
            .From($"{nameof(CustomProduct):raw} p")
            .InnerJoin($"{nameof(CustomProductType):raw} pt ON (p.{nameof(CustomProduct.TypeId):raw} = pt.{nameof(CustomProduct.Id):raw})")
            .Where($"p.{nameof(CustomProduct.Tag):raw} = {tag.DefineParam(DbType.String)}")
            .Where($"p.{nameof(CustomProduct.TypeId):raw} = {mySqlTestsFixture.SeedProductTypes[0].Id}");

        // Act
        var result = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByLeftJoinOnProductAndProductType_ReturnsIEnumerableOfCustomProduct()
    {
        // Arrange
        const string tag = "selectLeftJoin";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedCustomProductsAsync(connection, tag: tag)).ToList();
        products.AddRange(await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection, productTypeId: mySqlTestsFixture.SeedProductTypes[0].Id, tag: tag, productDescription: mySqlTestsFixture.SeedProductTypes[0].Description));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(CustomProductType.Description):raw}")
            .From($"{nameof(CustomProduct):raw} p")
            .LeftJoin($"{nameof(CustomProductType):raw} pt ON (p.{nameof(CustomProduct.TypeId):raw} = pt.{nameof(CustomProduct.Id):raw})")
            .WhereFilter($"p.{nameof(CustomProduct.Tag):raw} = {tag}");

        // Act
        var result = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByRightJoinOnProductTypeAndProduct_ReturnsIEnumerableOfCustomProduct()
    {
        // Arrange
        const int count = 3;
        const string tag = "selectRightJoin";
        const string tag2 = $"{tag}2";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedCustomProductsAsync(connection, count, tag: tag)).ToList();
        products.AddRange(await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection, count, mySqlTestsFixture.SeedProductTypes[0].Id, tag, mySqlTestsFixture.SeedProductTypes[0].Description));
        products.AddRange(await ProductHelpers.GenerateSeedCustomProductsAsync(
            connection, count, mySqlTestsFixture.SeedProductTypes[1].Id, tag2, mySqlTestsFixture.SeedProductTypes[1].Description));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(CustomProductType.Description):raw}")
            .From($"{nameof(CustomProductType):raw} pt")
            .RightJoin($"{nameof(CustomProduct):raw} p ON (p.{nameof(CustomProduct.TypeId):raw} = pt.{nameof(CustomProduct.Id):raw})")
            .WhereFilter().WithFilter($"p.{nameof(CustomProduct.Tag):raw} = {tag}").OrWhereFilter($"p.{nameof(CustomProduct.Tag):raw} = {tag2}");

        // Act
        var result = await connection.QueryAsync<CustomProduct>(builder.Sql, builder.Parameters);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Update_UpdatesProductWithUpdateTag_ReturnsInteger()
    {
        // Arrange
        const int count = 1;
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var product = (await ProductHelpers.GenerateSeedCustomProductsAsync(connection, count, tag: tag)).Single();

        var builder = SimpleBuilder.CreateFluent()
            .Update($"{nameof(CustomProduct):raw}")
            .Set(!product.TypeId.HasValue, $"{nameof(CustomProduct.TypeId):raw} = {mySqlTestsFixture.SeedProductTypes[0].Id}")
            .Set($"{nameof(CustomProduct.CreatedDate):raw} = {createdDate}")
            .WhereFilter($"{nameof(CustomProduct.Tag):raw} = {tag}").WithFilter($"{nameof(CustomProduct.TypeId):raw} IS NULL");

        var getUpdatedProduct = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"{nameof(CustomProduct):raw}")
            .Where($"{nameof(CustomProduct.Id):raw} = {product.Id}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        var updatedProduct = await connection.QuerySingleAsync<CustomProduct>(getUpdatedProduct.Sql, getUpdatedProduct.Parameters);
        updatedProduct.TypeId.Should().Be(mySqlTestsFixture.SeedProductTypes[0].Id);
        updatedProduct.CreatedDate.Should().Be(createdDate);
    }

    [Fact]
    public async Task Delete_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = mySqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedCustomProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"{nameof(CustomProduct):raw}")
            .Where($"{nameof(CustomProduct.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(CustomProduct):raw}")
            .Where($"{nameof(CustomProduct.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.Should().Be(count);

        var countResult = await connection.ExecuteScalarAsync<int>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);
        countResult.Should().Be(0);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => mySqlTestsFixture.ResetDatabaseAsync();
}
