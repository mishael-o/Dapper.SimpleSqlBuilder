using System.Data;
using Dapper.SimpleSqlBuilder.Extensions;
using Dapper.SimpleSqlBuilder.IntegrationTests.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MSSql;

[Collection(nameof(MSSqlTestsCollection))]
public class MSSqlFluentTests : IAsyncLifetime
{
    private readonly MSSqlTestsFixture mssqlTestsFixture;

    public MSSqlFluentTests(MSSqlTestsFixture mssqlTestsFixture)
    {
        this.mssqlTestsFixture = mssqlTestsFixture;
    }

    [Fact]
    public async Task Insert_AddsProducts_ReturnsInteger()
    {
        //Arrange
        const string tag = "insert";
        var product = ProductHelpers
            .GetProductFixture(tag: tag)
            .Create();

        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"{nameof(Product):raw}")
            .Columns($"{nameof(Product.Id):raw}")
            .Columns($"{nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}")
            .Columns($"{nameof(Product.CreatedDate):raw}")
            .Values($"{product.Id}")
            .Values($"{product.TypeId.DefineParam(DbType.Guid)}")
            .Values($"{product.Tag}, {product.CreatedDate}");

        var insertCountBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        //Assert
        var insertCount = await connection.ExecuteScalarAsync<int>(insertCountBuilder.Sql, insertCountBuilder.Parameters);
        result.Should().Be(1).And.Be(insertCount);
    }

    [Fact]
    public async Task Select_GetsProductsWithSelectTags_ReturnsIEnumerableOfProduct()
    {
        //Arrange
        const string tag = "select";
        const string tag2 = $"{tag}2";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedProductsAsync(
            connection,
            productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id,
            tag: tag,
            productDescription: mssqlTestsFixture.SeedProductTypes[0].Description)).ToList();

        products.AddRange(await ProductHelpers.GenerateSeedProductsAsync(connection, tag: tag2));

        FormattableString subQuery = $@"
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}";

        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery}) AS {nameof(Product.Description):raw}")
            .From($"{nameof(Product):raw} x")
            .Where($"{nameof(Product.Tag):raw} = {tag}")
            .OrWhere($"{nameof(Product.Tag):raw} = {tag2}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByInnerJoinProductAndProductType_ReturnsIEnumerableOfProduct()
    {
        //Arrange
        const string tag = "selectInnerJoin";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await ProductHelpers.GenerateSeedProductsAsync(
            connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag, productDescription: mssqlTestsFixture.SeedProductTypes[0].Description);
        await ProductHelpers.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[1].Id, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*, pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(Product):raw} p")
            .InnerJoin($"{nameof(ProductType):raw} pt ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .Where($"p.{nameof(Product.Tag):raw} = {tag}")
            .Where($"p.{nameof(Product.TypeId):raw} = {mssqlTestsFixture.SeedProductTypes[0].Id.DefineParam(DbType.Guid)}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByLeftJoinProductAndProductType_ReturnsIEnumerableOfProduct()
    {
        //Arrange
        const string tag = "selectLeftJoin";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedProductsAsync(connection, tag: tag)).ToList();
        products.AddRange(await ProductHelpers.GenerateSeedProductsAsync(
            connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag, productDescription: mssqlTestsFixture.SeedProductTypes[0].Description));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(Product):raw} p")
            .LeftJoin($"{nameof(ProductType):raw} pt ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .WhereFilter($"p.{nameof(Product.Tag):raw} = {tag}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Select_GetsProductsByRightJoinProductTypeAndProduct_ReturnsIEnumerableOfProduct()
    {
        //Arrange
        const int count = 3;
        const string tag = "selectRightJoin";
        const string tag2 = $"{tag}2";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = (await ProductHelpers.GenerateSeedProductsAsync(connection, count, tag: tag)).ToList();
        products.AddRange(await ProductHelpers.GenerateSeedProductsAsync(
            connection, count, mssqlTestsFixture.SeedProductTypes[0].Id, tag, mssqlTestsFixture.SeedProductTypes[0].Description));
        products.AddRange(await ProductHelpers.GenerateSeedProductsAsync(
            connection, count, mssqlTestsFixture.SeedProductTypes[1].Id, tag2, mssqlTestsFixture.SeedProductTypes[1].Description));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(ProductType):raw} pt")
            .RightJoin($"{nameof(Product):raw} p ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .WhereFilter().WithFilter($"p.{nameof(Product.Tag):raw} = {tag}").OrWhereFilter($"p.{nameof(Product.Tag):raw} = {tag2}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Update_UpdatesProductWithUpdateTag_ReturnsInteger()
    {
        //Arrange
        const int count = 1;
        const string tag = "update";
        var createdDate = DateTime.Now.AddDays(100).Date;

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var product = (await ProductHelpers.GenerateSeedProductsAsync(connection, count, tag: tag)).Single();

        var builder = SimpleBuilder.CreateFluent()
            .Update($"{nameof(Product):raw}")
            .Set(!product.TypeId.HasValue, $"{nameof(Product.TypeId):raw} = {mssqlTestsFixture.SeedProductTypes[0].Id}")
            .Set($"{nameof(Product.CreatedDate):raw} = {createdDate}")
            .WhereFilter($"{nameof(Product.Tag):raw} = {tag}").WithFilter($"{nameof(Product.TypeId):raw} IS NULL");

        var getUpdatedProduct = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Id):raw} = {product.Id}");

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        //Assert
        result.Should().Be(count);

        var updatedProduct = await connection.QuerySingleAsync<Product>(getUpdatedProduct.Sql, getUpdatedProduct.Parameters);
        updatedProduct.TypeId.Should().Be(mssqlTestsFixture.SeedProductTypes[0].Id);
        updatedProduct.CreatedDate.Should().Be(createdDate);
    }

    [Fact]
    public async Task Delete_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        //Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        await ProductHelpers.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        //Assert
        result.Should().Be(count);

        var countResult = await connection.ExecuteScalarAsync<int>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);
        countResult.Should().Be(0);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => mssqlTestsFixture.ResetDatabaseAsync();
}
