using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Dapper;
using Dapper.SimpleSqlBuilder;

namespace SimpleSqlBuilder.BenchMark.Benchmarks;

[MemoryDiagnoser]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class SimpleSqlBuilderBenchmark
{
    private const int WhereOperationCount = 20;

    private Product product = default!;
    private int[] typeIds = default!;

    [GlobalSetup]
    public void GlobalSetUp()
    {
        product = new()
        {
            Id = Guid.NewGuid(),
            ProductCode = "Product Code",
            RecommendedPrice = 100.00,
            SellingPrice = 99.99m
        };

        typeIds = [.. Enumerable.Range(1, 10)];
    }

    [Benchmark(Description = "SqlBuilder (Dapper)", Baseline = true)]
    [BenchmarkCategory("Simple query")]
    public string SqlBuilder()
    {
        const string sql = $"""
            SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = @{nameof(Product.Id)} OR ProductCode = @{nameof(Product.ProductCode)})
            FROM Product x
            /**where**/
            """;

        var sqlBuilder = new SqlBuilder()
            .Where($"Id = @{nameof(Product.Id)}", new { product.Id })
            .Where($"ProductCode = @{nameof(Product.ProductCode)}", new { product.ProductCode })
            .Where($"TypeId IN @{nameof(typeIds)}", new { typeIds })
            .Where($"RecommendedPrice = @{nameof(Product.RecommendedPrice)}", new { product.RecommendedPrice })
            .Where($"SellingPrice = @{nameof(Product.SellingPrice)}", new { product.SellingPrice })
            .Where($"IsActive = @{nameof(Product.IsActive)}", new { product.IsActive })
            .Where($"CreateDate = @{nameof(Product.CreateDate)}", new { product.CreateDate });

        var template = sqlBuilder.AddTemplate(sql);
        return template.RawSql;
    }

    [Benchmark(Description = "Builder")]
    [BenchmarkCategory("Simple query")]
    public string SimpleSqlBuilder()
    {
        var builder = SimpleBuilder.Create(
            $"""
               SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})
               FROM Product x
               WHERE Id = {product.Id}
               AND ProductCode = {product.ProductCode}
               AND TypeId IN {typeIds}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}
               """);

        return builder.Sql;
    }

    [Benchmark(Description = "FluentBuilder")]
    [BenchmarkCategory("Simple query")]
    public string SimpleSqlFluentBuilder()
    {
        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})")
            .From($"Product x")
            .Where($"Id = {product.Id}")
            .Where($"ProductCode = {product.ProductCode}")
            .Where($"TypeId IN {typeIds}")
            .Where($"RecommendedPrice = {product.RecommendedPrice}")
            .Where($"SellingPrice = {product.SellingPrice}")
            .Where($"IsActive = {product.IsActive}")
            .Where($"CreateDate = {product.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "Builder (Reuse parameters)")]
    [BenchmarkCategory("Simple query")]
    public string SimpleSqlBuilderReuseParameters()
    {
        ////AND Name = { product.Name }

        var builder = SimpleBuilder.Create(reuseParameters: true)
            .AppendIntact($"""
               SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})
               FROM Product x
               WHERE Id = {product.Id}
               AND ProductCode = {product.ProductCode}
               AND TypeId IN {typeIds}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}
               """);

        return builder.Sql;
    }

    [Benchmark(Description = "FluentBuilder (Reuse parameters)")]
    [BenchmarkCategory("Simple query")]
    public string SimpleSqlFluentBuilderReuseParameters()
    {
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Select($"x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})")
            .From($"Product x")
            .Where($"Id = {product.Id}")
            .Where($"ProductCode = {product.ProductCode}")
            .Where($"TypeId IN {typeIds}")
            .Where($"RecommendedPrice = {product.RecommendedPrice}")
            .Where($"SellingPrice = {product.SellingPrice}")
            .Where($"IsActive = {product.IsActive}")
            .Where($"CreateDate = {product.CreateDate}");

        return builder.Sql;
    }

    [Benchmark(Description = "SqlBuilder (Dapper)", Baseline = true)]
    [BenchmarkCategory("Large query")]
    public string SqlBuilderLarge()
    {
        const string sql = $"""
            SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = @{nameof(Product.Id)} OR ProductCode = @{nameof(Product.ProductCode)})
            FROM Product x
            /**where**/
            """;

        var sqlBuilder = new SqlBuilder();

        // Simulating large query
        for (var i = 0; i < WhereOperationCount; i++)
        {
            sqlBuilder
                .Where($"Id = @{nameof(Product.Id)}", new { product.Id })
                .Where($"ProductCode = @{nameof(Product.ProductCode)}", new { product.ProductCode })
                .Where($"TypeId IN @{nameof(typeIds)}", new { typeIds })
                .Where($"RecommendedPrice = @{nameof(Product.RecommendedPrice)}", new { product.RecommendedPrice })
                .Where($"SellingPrice = @{nameof(Product.SellingPrice)}", new { product.SellingPrice })
                .Where($"IsActive = @{nameof(Product.IsActive)}", new { product.IsActive })
                .Where($"CreateDate = @{nameof(Product.CreateDate)}", new { product.CreateDate });
        }

        var template = sqlBuilder.AddTemplate(sql);
        return template.RawSql;
    }

    [Benchmark(Description = "Builder")]
    [BenchmarkCategory("Large query")]
    public string SimpleSqlBuilderLarge()
    {
        var builder = SimpleBuilder.Create(
            $"""
               SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})
               FROM Product x
               WHERE 1 = 1
               """);

        // Simulating large query
        for (var i = 0; i < WhereOperationCount; i++)
        {
            builder.Append($"""
               AND Id = {product.Id}
               AND ProductCode = {product.ProductCode}
               AND TypeId IN {typeIds}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}
               """);
        }

        return builder.Sql;
    }

    [Benchmark(Description = "FluentBuilder")]
    [BenchmarkCategory("Large query")]
    public string SimpleSqlFluentBuilderLarge()
    {
        var builder = SimpleBuilder.CreateFluent()
            .Select($"x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})")
            .From($"Product x");

        // Simulating large query
        for (var i = 0; i < WhereOperationCount; i++)
        {
            builder
                .Where($"Id = {product.Id}")
                .Where($"ProductCode = {product.ProductCode}")
                .Where($"TypeId IN {typeIds}")
                .Where($"RecommendedPrice = {product.RecommendedPrice}")
                .Where($"SellingPrice = {product.SellingPrice}")
                .Where($"IsActive = {product.IsActive}")
                .Where($"CreateDate = {product.CreateDate}");
        }

        return builder.Sql;
    }

    [Benchmark(Description = "Builder (Reuse parameters)")]
    [BenchmarkCategory("Large query")]
    public string SimpleSqlBuilderLargeReuseParameters()
    {
        var builder = SimpleBuilder.Create(reuseParameters: true)
            .AppendIntact($"""
               SELECT x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})
               FROM Product x
               WHERE 1 = 1
               """);

        // Simulating large query
        for (var i = 0; i < WhereOperationCount; i++)
        {
            builder.Append($"""
               AND Id = {product.Id}
               AND ProductCode = {product.ProductCode}
               AND TypeId IN {typeIds}
               AND RecommendedPrice = {product.RecommendedPrice}
               AND SellingPrice = {product.SellingPrice}
               AND IsActive = {product.IsActive}
               AND CreateDate = {product.CreateDate}
               """);
        }

        return builder.Sql;
    }

    [Benchmark(Description = "FluentBuilder (Reuse parameters)")]
    [BenchmarkCategory("Large query")]
    public string SimpleSqlFluentBuilderLargeReuseParameters()
    {
        var builder = SimpleBuilder.CreateFluent(reuseParameters: true)
            .Select($"x.*, (SELECT Description FROM ProductDetail WHERE Id = {product.Id} OR ProductCode = {product.ProductCode})")
            .From($"Product x");

        // Simulating large query
        for (var i = 0; i < WhereOperationCount; i++)
        {
            builder
                .Where($"Id = {product.Id}")
                .Where($"ProductCode =  {product.ProductCode}")
                .Where($"TypeId IN {typeIds}")
                .Where($"RecommendedPrice = {product.RecommendedPrice}")
                .Where($"SellingPrice = {product.SellingPrice}")
                .Where($"IsActive = {product.IsActive}")
                .Where($"CreateDate = {product.CreateDate}");
        }

        return builder.Sql;
    }
}
