namespace SimpleSqlBuilder.BenchMark.Benchmarks;

public record Product(Guid Id, int TypeId, string Description, double RecommendedPrice, decimal SellingPrice, bool IsActive, DateTimeOffset CreateDate);
