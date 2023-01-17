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
    public async Task InsertDataInTable_ValidateInsert()
    {
        //Arrange
        const string tag = "insert";
        var product = Helpers
            .GetBaseProductComposer(mssqlTestsFixture.DefaultProductType.Id, tag)
            .Create();

        var builder = SimpleBuilder.CreateFluent()
            .InsertInto($"{nameof(Product):raw}")
            .Columns($"{nameof(Product.Id):raw}")
            .Columns($"{nameof(Product.TypeId):raw}")
            .Columns($"{nameof(Product.Tag):raw}")
            .Columns($"{nameof(Product.CreatedDate):raw}")
            .Values($"{product.Id}")
            .Values($"{product.TypeId.DefineParam(DbType.Guid)}")
            .Values($"{product.Tag}")
            .Values($"{product.CreatedDate}");

        var insertCountBuilder = SimpleBuilder.CreateFluent()
            .Select($"COUNT(*)")
            .From($"{nameof(Product):raw}")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        //Act
        var result = await connection.ExecuteAsync(builder.Sql, builder.Parameters);
        var insertCount = await connection.ExecuteScalarAsync<int>(insertCountBuilder.Sql, insertCountBuilder.Parameters);

        //Assert
        result.Should().Be(1).And.Be(insertCount);
    }

    [Fact]
    public async Task GetDataInTable_ValidateSelect()
    {
        //Arrange
        const string tag = "select";

        using var connection = mssqlTestsFixture.CreateDbConnection();
        await connection.OpenAsync();

        var products = await Helpers.GenerateSeedProductsDataAsync(
            mssqlTestsFixture.DefaultProductType.Id,
            connection,
            tag: tag,
            productDescription: mssqlTestsFixture.DefaultProductType.Description);

        FormattableString subQuery = $@"
            SELECT {nameof(ProductType.Description):raw}
            FROM {nameof(ProductType):raw}
            WHERE {nameof(ProductType.Id):raw} = x.{nameof(Product.TypeId):raw}";

        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*")
            .Select($"({subQuery}) AS {nameof(Product.Description):raw}")
            .From($"{nameof(Product):raw} x")
            .Where($"{nameof(Product.Tag):raw} = {tag}");

        //Act
        var result = await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);

        //Assert
        result.Should().BeEquivalentTo(products);
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public Task DisposeAsync()
        => mssqlTestsFixture.ResetDatabaseAsync();
}
