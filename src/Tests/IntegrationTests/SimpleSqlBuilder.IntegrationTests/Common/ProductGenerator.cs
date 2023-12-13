using System.Data.Common;
using AutoFixture.Dsl;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

internal static class ProductGenerator
{
    public static async Task<IEnumerable<Product>> GenerateSeedProductsAsync(DbConnection connection, int count = 5, int? productTypeId = null, string? tag = null)
    {
        var products = GetProductFixture(productTypeId, tag)
            .CreateMany(count)
            .ToArray();

        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.AppendIntact($"""
                INSERT INTO {nameof(Product):raw} ({nameof(Product.GlobalId):raw}, {nameof(Product.TypeId):raw}, {nameof(Product.Tag):raw}, {nameof(Product.CreatedDate):raw})
                VALUES ({product.GlobalId}, {product.TypeId}, {product.Tag}, {product.CreatedDate});
                """).AppendNewLine();
        }

        var globalIds = products.Select(x => x.GlobalId).ToArray();
        FormattableString whereCondition = connection is Npgsql.NpgsqlConnection
            ? $"= ANY({globalIds})"
            : (FormattableString)$"IN {globalIds}";

        builder
            .AppendNewLine($"SELECT x.*")
            .AppendIntact(productTypeId.HasValue, $", (SELECT {nameof(ProductType.Description):raw} FROM {nameof(ProductType):raw} WHERE {nameof(ProductType.Id):raw} = {productTypeId}) AS {nameof(ProductType.Description):raw}")
            .AppendNewLine($"""
                FROM {nameof(Product):raw} x
                WHERE {nameof(Product.GlobalId):raw} {whereCondition}
                ORDER BY {nameof(Product.Id):raw};
                """);

        return await connection.QueryAsync<Product>(builder.Sql, builder.Parameters);
    }

    public static IPostprocessComposer<Product> GetProductFixture(int? productTypeId = null, string? tag = null)
    {
        var fixture = new Fixture()
            .Build<Product>()
            .Without(x => x.Id);

        fixture = productTypeId.HasValue
            ? fixture.With(x => x.TypeId, productTypeId.Value)
            : fixture.Without(x => x.TypeId);

        fixture = string.IsNullOrWhiteSpace(tag)
            ? fixture.Without(x => x.Tag)
            : fixture.With(x => x.Tag, tag);

        return fixture;
    }
}
