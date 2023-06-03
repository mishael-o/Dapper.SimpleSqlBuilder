using System.Data.Common;
using AutoFixture.Dsl;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

internal static class ProductHelpers
{
    public static async Task<IEnumerable<Product>> GenerateSeedProductsAsync(
        DbConnection connection,
        int count = 5,
        Guid? productTypeId = null,
        string? tag = null,
        string? productDescription = null)
    {
        var products = GetProductFixture(productTypeId, tag, productDescription)
            .CreateMany(count)
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.AppendNewLine(
               $@"INSERT INTO {nameof(Product):raw} ({nameof(Product.Id):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
               VALUES ({product.Id}, {product.TypeId}, {product.Tag}, {product.CreatedDate});");
        }

        await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        return products;
    }

    public static async Task<IEnumerable<CustomProduct>> GenerateSeedCustomProductsAsync(
        DbConnection connection,
        int count = 5,
        CustomId? productTypeId = null,
        string? tag = null,
        string? productDescription = null)
    {
        var products = GetCustomProductFixture(productTypeId, tag, productDescription)
            .CreateMany(count)
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.AppendNewLine(
               $@"INSERT INTO {nameof(CustomProduct):raw} ({nameof(CustomProduct.Id):raw}, {nameof(CustomProduct.TypeId):raw}, {nameof(CustomProduct.Tag):raw}, {nameof(CustomProduct.CreatedDate):raw})
                   VALUES ({product.Id}, {product.TypeId}, {product.Tag}, {product.CreatedDate});");
        }

        await connection.ExecuteAsync(builder.Sql, builder.Parameters);

        return products;
    }

    public static IPostprocessComposer<Product> GetProductFixture(Guid? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<Product>();

        var composer = fixture
            .With(x => x.CreatedDate, () => fixture.Create<DateTime>().Date);

        composer = productTypeId.HasValue
            ? composer.With(x => x.TypeId, productTypeId.Value)
            : composer.Without(x => x.TypeId);

        composer = string.IsNullOrWhiteSpace(productDescription)
            ? composer.Without(x => x.Description)
            : composer.With(x => x.Description, productDescription);

        composer = string.IsNullOrWhiteSpace(tag)
            ? composer.Without(x => x.Tag)
            : composer.With(x => x.Tag, tag);

        return composer;
    }

    public static IPostprocessComposer<CustomProduct> GetCustomProductFixture(CustomId? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<CustomProduct>();

        var composer = fixture
            .With(x => x.CreatedDate, () => fixture.Create<DateTime>().Date);

        composer = productTypeId.HasValue
            ? composer.With(x => x.TypeId, productTypeId.Value)
            : composer.Without(x => x.TypeId);

        composer = string.IsNullOrWhiteSpace(productDescription)
            ? composer.Without(x => x.Description)
            : composer.With(x => x.Description, productDescription);

        composer = string.IsNullOrWhiteSpace(tag)
            ? composer.Without(x => x.Tag)
            : composer.With(x => x.Tag, tag);

        return composer;
    }
}
