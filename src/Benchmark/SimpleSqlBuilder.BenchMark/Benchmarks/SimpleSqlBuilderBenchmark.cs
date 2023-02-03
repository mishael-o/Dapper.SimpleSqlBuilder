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

    private Product testProduct = default!;
    private IEnumerable<Product> testProducts = default!;

    [GlobalSetup]
    public void GlobalSetUp()
    {
        var fixture = new Fixture();
        testProduct = fixture.Create<Product>();
        testProducts = CreateProducts(fixture, testProduct);
    }

    [Benchmark(Description = "SqlBuilder (Dapper) - Simple query")]
    public string SqlBuilder()
    {
        const string sql = $"SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE Id = @{nameof(testProduct.TypeId)}) FROM Product x /**where**/";

        var sqlBuilder = new SqlBuilder()
            .Where($"Id = @{nameof(Product.Id)}", new { testProduct.Id })
            .Where($"TypeId = @{nameof(Product.TypeId)}", new { testProduct.TypeId })
            .Where($"RecommendedPrice = @{nameof(Product.RecommendedPrice)}", new { testProduct.RecommendedPrice })
            .Where($"SellingPrice = @{nameof(Product.SellingPrice)}", new { testProduct.SellingPrice })
            .Where($"IsActive = @{nameof(Product.IsActive)}", new { testProduct.IsActive })
            .Where($"CreateDate = @{nameof(Product.CreateDate)}", new { testProduct.CreateDate });

        var template = sqlBuilder.AddTemplate(sql);
        return template.RawSql;
    }

    [Benchmark(Description = "SimpleSqlBuilder - Simple query")]
    public string SimpleSqlBuilder()
    {
        var builder = SimpleBuilder.Create($@"
               SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE ID = {testProduct.TypeId})
               FROM Product x
               WHERE Id = {testProduct.Id}
               AND TypeId = {testProduct.TypeId}
               AND RecommendedPrice = {testProduct.RecommendedPrice}
               AND SellingPrice = {testProduct.SellingPrice}
               AND IsActive = {testProduct.IsActive}
               AND CreateDate = {testProduct.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "SimpleSqlBuilder - Simple query (Reuse parameters)")]
    public string SimpleSqlBuilderReuseParameters()
    {
        var builder = SimpleBuilder.Create(reuseParameters: true).Append($@"
               SELECT x.*, (SELECT DESCRIPTION FROM ProductType WHERE ID = {testProduct.TypeId})
               FROM Product x
               WHERE Id = {testProduct.Id}
               AND TypeId = {testProduct.TypeId}
               AND RecommendedPrice = {testProduct.RecommendedPrice}
               AND SellingPrice = {testProduct.SellingPrice}
               AND IsActive = {testProduct.IsActive}
               AND CreateDate = {testProduct.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "SqlBuilder (Dapper) - Large query")]
    public string SqlBuilderLarge()
    {
        const string sql = "UPDATE Product /**set**/ /**where**/";

        //object to hold the queries created by SqlBuilder.
        var stringBuilder = new StringBuilder();

        var sqlBuilder = new SqlBuilder()
            .Set($"TypeId = @{nameof(Product.TypeId)}")
            .Set($"RecommendedPrice = @{nameof(Product.RecommendedPrice)}")
            .Set($"SellingPrice = @{nameof(Product.SellingPrice)}")
            .Set($"IsActive = @{nameof(Product.IsActive)}")
            .Set($"CreateDate = @{nameof(Product.CreateDate)}")
            .Where($"Id = @{nameof(Product.Id)}");

        foreach (var prd in testProducts)
        {
            var template = sqlBuilder.AddTemplate(sql, prd);
            stringBuilder.Append(template.RawSql);
        }

        return stringBuilder.ToString();
    }

    [Benchmark(Description = "SimpleSqlBuilder - Large query")]
    public string SimpleSqlBuilderLarge()
    {
        var builder = SimpleBuilder.Create();

        foreach (var product in testProducts)
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

        foreach (var product in testProducts)
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
