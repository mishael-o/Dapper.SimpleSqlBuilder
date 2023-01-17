using System.Data.Common;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

internal static class Helpers
{
    public static async Task<IEnumerable<Product>> GenerateSeedProductsDataAsync(Guid productTypeId, DbConnection connection, int count = 5, string? tag = null, string? productDescription = null)
    {
        var products = GetBaseProductComposer(productTypeId, tag, productDescription)
            .With(x => x.CreatedDate, DateTime.Now.Date)
            .CreateMany(count);

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

    public static async Task<IEnumerable<CustomProduct>> GenerateSeedCustomProductsAsync(CustomId productTypeId, DbConnection connection, int count = 5, string? tag = null, string? productDescription = null)
    {
        var products = GetBaseCustomProductComposer(productTypeId, tag, productDescription)
            .With(x => x.CreatedDate, DateTime.Now.Date)
            .CreateMany(count);

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

    public static AutoFixture.Dsl.IPostprocessComposer<Product> GetBaseProductComposer(Guid productTypeId, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<Product>()
            .With(x => x.TypeId, productTypeId)
            .With(x => x.CreatedDate, DateTime.Now.Date);

        if (!string.IsNullOrWhiteSpace(productDescription))
        {
            fixture = fixture.With(x => x.Description, productDescription);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            fixture = fixture.With(x => x.Tag, tag);
        }

        return fixture;
    }

    public static AutoFixture.Dsl.IPostprocessComposer<CustomProduct> GetBaseCustomProductComposer(CustomId productTypeId, string? tag = null, string? productDescription = null)
    {
        var fixture = new Fixture()
            .Build<CustomProduct>()
            .With(x => x.TypeId, productTypeId)
            .With(x => x.CreatedDate, DateTime.Now.Date);

        if (!string.IsNullOrWhiteSpace(productDescription))
        {
            fixture = fixture.With(x => x.Description, productDescription);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            fixture = fixture.With(x => x.Tag, tag);
        }

        return fixture;
    }
}
