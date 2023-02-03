namespace SimpleSqlBuilder.BenchMark.Benchmarks;

public record Product(Guid Id, int TypeId, double RecommendedPrice, decimal SellingPrice, bool IsActive, DateTimeOffset CreateDate);
