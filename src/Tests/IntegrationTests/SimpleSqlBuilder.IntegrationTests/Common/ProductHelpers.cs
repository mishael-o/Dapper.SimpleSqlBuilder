using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

internal static class ProductHelpers
{
    public static async Task<IEnumerable<Product>> GenerateSeedProductsAsync(DbConnection connection, int count = 5, Guid? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var products = GetProductFixture(productTypeId, tag, productDescription)
            .With(x => x.CreatedDate, DateTime.Now.Date)
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

    public static async Task<IEnumerable<CustomProduct>> GenerateSeedCustomProductsAsync(DbConnection connection, int count = 5, CustomId? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var products = GetCustomProductFixture(productTypeId, tag, productDescription)
            .With(x => x.CreatedDate, DateTime.Now.Date)
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

    public static AutoFixture.Dsl.IPostprocessComposer<Product> GetProductFixture(Guid? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<Product>()
            .With(x => x.CreatedDate, DateTime.Now.Date);

        fixture = productTypeId.HasValue
            ? fixture.With(x => x.TypeId, productTypeId.Value)
            : fixture.Without(x => x.TypeId);

        fixture = string.IsNullOrWhiteSpace(productDescription)
            ? fixture.Without(x => x.Description)
            : fixture.With(x => x.Description, productDescription);

        fixture = string.IsNullOrWhiteSpace(tag)
            ? fixture.Without(x => x.Tag)
            : fixture.With(x => x.Tag, tag);

        return fixture;
    }

    public static AutoFixture.Dsl.IPostprocessComposer<CustomProduct> GetCustomProductFixture(CustomId? productTypeId = null, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<CustomProduct>()
            .With(x => x.CreatedDate, DateTime.Now.Date);

        fixture = productTypeId.HasValue
            ? fixture.With(x => x.TypeId, productTypeId.Value)
            : fixture.Without(x => x.TypeId);

        fixture = string.IsNullOrWhiteSpace(productDescription)
            ? fixture.Without(x => x.Description)
            : fixture.With(x => x.Description, productDescription);

        fixture = string.IsNullOrWhiteSpace(tag)
            ? fixture.Without(x => x.Tag)
            : fixture.With(x => x.Tag, tag);

        return fixture;
    }
}
