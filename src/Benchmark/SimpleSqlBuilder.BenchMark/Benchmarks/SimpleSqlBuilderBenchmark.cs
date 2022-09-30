using System.Security.Cryptography;
using System.Text;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using Dapper;
using Dapper.SimpleSqlBuilder;

namespace SimpleSqlBuilder.BenchMark.Benchmarks;

[MemoryDiagnoser]
public class SimpleSqlBuilderBenchmark
{
    private const int ProductCount = 100;

    private Product product = default!;
    private IEnumerable<Product> products = default!;

    [GlobalSetup]
    public void GlobalSetUp()
    {
        var fixture = new Fixture();
        product = fixture.Create<Product>();
        products = CreateProducts(fixture, product);
    }

    [Benchmark(Description = "SqlBuilder (Dapper) - Simple query")]
    public string SqlBuilder()
    {
        const string sql = $"SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE Id = @{nameof(product.TypeId)}) FROM Product x /**where**/";

        var sqlBuilder = new SqlBuilder()
            .Where($"Id = @{nameof(product.Id)}", new { product.Id })
            .Where($"TypeId = @{nameof(product.TypeId)}", new { product.TypeId })
            .Where($"RecommendedPrice = @{nameof(product.RecommendedPrice)}", new { product.RecommendedPrice })
            .Where($"SellingPrice = @{nameof(product.SellingPrice)}", new { product.SellingPrice })
            .Where($"IsActive = @{nameof(product.IsActive)}", new { product.IsActive })
            .Where($"CreateDate = @{nameof(product.CreateDate)}", new { product.CreateDate });

        var template = sqlBuilder.AddTemplate(sql);
        return template.RawSql;
    }

    [Benchmark(Description = "SimpleSqlBuilder - Simple query")]
    public string SimpleSqlBuilder()
    {
        var builder = SimpleBuilder.Create($@"
               SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE ID = {product.TypeId})
               FROM Product x
               WHERE Id = {product.Id}
               AND TypeId = {product.TypeId}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "SimpleSqlBuilder - Simple query (Reuse parameters)")]
    public string SimpleSqlBuilderReuseParameters()
    {
        var builder = SimpleBuilder.Create(reuseParameters: true).Append($@"
               SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE ID = {product.TypeId})
               FROM Product x
               WHERE Id = {product.Id}
               AND TypeId = {product.TypeId}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "SqlBuilder (Dapper) - Large query")]
    public string SqlBuilderLarge()
    {
        const string sql = "UPDATE Product /**set**/ /**where**/";

        //object to hold the queries created by SqlBuilder.
        var stringBuilder = new StringBuilder();

        var sqlBuilder = new SqlBuilder()
            .Set($"TypeId = @{nameof(product.TypeId)}")
            .Set($"RecommendedPrice = @{nameof(product.RecommendedPrice)}")
            .Set($"SellingPrice = @{nameof(product.SellingPrice)}")
            .Set($"IsActive = @{nameof(product.IsActive)}")
            .Set($"CreateDate = @{nameof(product.CreateDate)}")
            .Where($"Id = @{nameof(product.Id)}");

        foreach (var product in products)
        {
            var template = sqlBuilder.AddTemplate(sql, product);
            stringBuilder.Append(template.RawSql);
        }

        return stringBuilder.ToString();
    }

    [Benchmark(Description = "SimpleSqlBuilder - Large query")]
    public string SimpleSqlBuilderLarge()
    {
        var builder = SimpleBuilder.Create();

        foreach (var product in products)
        {
            builder.Append(@$"
                UPDATE Product
                SET TypeId = {product.TypeId},
                RecommendedPrice = {product.RecommendedPrice},
                SellingPrice = {product.SellingPrice},
                IsActive = {product.IsActive},
                CreateDate = {product.CreateDate}
                WHERE Id = {product.Id}")
                .AppendNewLine();
        }

        return builder.Sql;
    }

    [Benchmark(Description = "SimpleSqlBuilder - Large query (Reuse parameters)")]
    public string SimpleSqlBuilderLargeReuseParameters()
    {
        var builder = SimpleBuilder.Create(reuseParameters: true);

        foreach (var product in products)
        {
            builder.Append(@$"
                UPDATE Product
                SET TypeId = {product.TypeId},
                RecommendedPrice = {product.RecommendedPrice},
                SellingPrice = {product.SellingPrice},
                IsActive = {product.IsActive},
                CreateDate = {product.CreateDate}
                WHERE Id = {product.Id}")
                .AppendNewLine();
        }

        return builder.Sql;
    }

    private static IEnumerable<Product> CreateProducts(Fixture fixture, Product templateProduct)
    {
        const int repeatCount = 25;

        var products = new List<Product>();

        products.AddRange(
            fixture.Build<Product>()
            .With(x => x.TypeId, templateProduct.TypeId)
            .CreateMany(repeatCount));

        products.AddRange(
            fixture.Build<Product>()
            .With(x => x.RecommendedPrice, templateProduct.RecommendedPrice)
            .CreateMany(repeatCount));

        products.AddRange(
            fixture.Build<Product>()
            .With(x => x.SellingPrice, templateProduct.SellingPrice)
            .CreateMany(repeatCount));

        products.AddRange(
            fixture.Build<Product>()
            .With(x => x.CreateDate, templateProduct.CreateDate)
            .CreateMany(repeatCount));

        return products.OrderBy(_ => RandomNumberGenerator.GetInt32(ProductCount)).ToList();
    }
}
