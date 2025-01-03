namespace SimpleSqlBuilder.BenchMark.Benchmarks;

internal sealed record Product
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = default!;
    public double RecommendedPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
}
