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
        // Arrange
        const string tag = "insert";
        var product = ProductGenerator
            .GetProductFixture(tag: tag)
            .Create();

        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"{nameof(Product):raw}")
            .Columns($"{nameof(Product.Id):raw}")
            .Columns($"{nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}")
            .Columns($"{nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw}")
            .Values($"{product.Id}")
            .Values($"{product.GlobalId}, {product.TypeId.DefineParam(DbType.Int32)}")
            .Values($"{product.Tag}, {product.CreatedDate.DefineParam(DbType.DateTime2)}");

        var insertCountBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        var insertCount = await connection.ExecuteScalarAsync<int>(insertCountBuilder.Sql, insertCountBuilder.Parameters);
        result.ShouldBe(1);
        result.ShouldBe(insertCount);
    }

    [Fact]
    public async Task Select_GetsProductsWithSelectTag_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const string tag = "select";
        const string tag2 = $"{tag}2";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = (await ProductGenerator.GenerateSeedProductsAsync(
            connection,
            productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id,
            tag: tag)).ToList();

        products.AddRange(await ProductGenerator.GenerateSeedProductsAsync(connection, tag: tag2));

        FormattableString subQuery = $"""
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}
            """;

        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery}) AS {nameof(Product.Description):raw}")
            .From($"{nameof(Product):raw} x")
            .Where($"{nameof(Product.Tag):raw} = {tag}")
            .OrWhere($"{nameof(Product.Tag):raw} = {tag2}");

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products, ignoreOrder: true);
    }

    [Fact]
    public async Task Select_GetsProductsByOffsetRows_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const int count = 10;
        const string tag = "offset";
        const int offset = 6;

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var paginatedProducts = products
            .OrderBy(x => x.CreatedDate)
            .ThenBy(x => x.Id)
            .Skip(offset);

        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}")
            .OrderBy($"{nameof(Product.CreatedDate):raw} ASC")
            .OrderBy($"{nameof(Product.Id):raw} ASC")
            .OffsetRows(offset);

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(paginatedProducts);
    }

    [Fact]
    public async Task Select_GetsProductsByOffsetRowAndFetchNext_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const int count = 30;
        const string tag = "page";
        const int offset = 5;
        const int rows = 10;

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var paginatedProducts = products
            .OrderBy(x => x.CreatedDate)
            .ThenBy(x => x.Id)
            .Skip(offset)
            .Take(rows);

        var builder = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}")
            .OrderBy($"{nameof(Product.CreatedDate):raw} ASC")
            .OrderBy($"{nameof(Product.Id):raw} ASC")
            .OffsetRows(offset)
            .FetchNext(rows);

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(paginatedProducts);
    }

    [Fact]
    public async Task Select_GetsProductsByInnerJoinOnProductAndProductType_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const string tag = "selectInnerJoin";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = await ProductGenerator.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag);
        await ProductGenerator.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[1].Id, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*, pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(Product):raw} p")
            .InnerJoin($"{nameof(ProductType):raw} pt ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .Where($"p.{nameof(Product.Tag):raw} = {tag}")
            .Where($"p.{nameof(Product.TypeId):raw} = {mssqlTestsFixture.SeedProductTypes[0].Id.DefineParam(DbType.Int32)}");

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products, ignoreOrder: true);
    }

    [Fact]
    public async Task Select_GetsProductsByLeftJoinOnProductAndProductType_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const string tag = "selectLeftJoin";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = (await ProductGenerator.GenerateSeedProductsAsync(connection, tag: tag)).ToList();
        products.AddRange(await ProductGenerator.GenerateSeedProductsAsync(connection, productTypeId: mssqlTestsFixture.SeedProductTypes[0].Id, tag: tag));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(Product):raw} p")
            .LeftJoin($"{nameof(ProductType):raw} pt ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .WhereFilter($"p.{nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products, ignoreOrder: true);
    }

    [Fact]
    public async Task Select_GetsProductsByRightJoinOnProductTypeAndProduct_ReturnsIEnumerableOfProduct()
    {
        // Arrange
        const int count = 3;
        const string tag = "selectRightJoin";
        const string tag2 = $"{tag}2";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var products = (await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag)).ToList();
        products.AddRange(await ProductGenerator.GenerateSeedProductsAsync(connection, count, mssqlTestsFixture.SeedProductTypes[0].Id, tag));
        products.AddRange(await ProductGenerator.GenerateSeedProductsAsync(connection, count, mssqlTestsFixture.SeedProductTypes[1].Id, tag2));

        var builder = SimpleBuilder.CreateFluent()
            .Select($"p.*")
            .Select($"pt.{nameof(ProductType.Description):raw}")
            .From($"{nameof(ProductType):raw} pt")
            .RightJoin($"{nameof(Product):raw} p ON (p.{nameof(Product.TypeId):raw} = pt.{nameof(Product.Id):raw})")
            .WhereFilter().WithFilter($"p.{nameof(Product.Tag):raw} = {tag}").OrWhereFilter($"p.{nameof(Product.Tag):raw} = {tag2}");

        // Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(products, ignoreOrder: true);
    }

    [Fact]
    public async Task Update_UpdatesProductWithUpdateTag_ReturnsInteger()
    {
        // Arrange
        const int count = 1;
        const string tag = "update";

        var createdDate = DateTime.UtcNow.AddDays(10);
        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        var product = (await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag)).Single();

        var builder = SimpleBuilder.CreateFluent()
            .Update($"{nameof(Product):raw}")
            .Set($"{nameof(Product.TypeId):raw} = {mssqlTestsFixture.SeedProductTypes[0].Id}")
            .Set($"{nameof(Product.CreatedDate):raw} = {createdDate.DefineParam(DbType.DateTime2)}")
            .WhereFilter($"{nameof(Product.Tag):raw} = {tag}").WithFilter($"{nameof(Product.TypeId):raw} IS NULL");

        var getUpdatedProduct = SimpleBuilder.CreateFluent()
            .Select($"*")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Id):raw} = {product.Id}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(count);

        var updatedProduct = await connection.QuerySingleAsync<Product>(getUpdatedProduct.Sql, getUpdatedProduct.Parameters);
        updatedProduct.TypeId.ShouldBe(mssqlTestsFixture.SeedProductTypes[0].Id);
        updatedProduct.CreatedDate.ShouldBe(createdDate);
    }

    [Fact]
    public async Task Delete_DeletesProductsWithDeleteTag_ReturnsInteger()
    {
        // Arrange
        const int count = 3;
        const string tag = "delete";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await ProductGenerator.GenerateSeedProductsAsync(connection, count, tag: tag);

        var builder = SimpleBuilder.CreateFluent()
            .DeleteFrom($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        var checkDataExistsBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        // Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        // Assert
        result.ShouldBe(count);

        var countResult = await connection.ExecuteScalarAsync<int>(checkDataExistsBuilder.Sql, checkDataExistsBuilder.Parameters);
        countResult.ShouldBe(0);
    }

    public ValueTask InitializeAsync()
        => default;

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return new(mssqlTestsFixture.ResetDatabaseAsync());
    }
}
