namespace SimpleSqlBuilder.BenchMark.Benchmarks;

public record Product
{
    public Guid Id { get; set; }
    public int TypeId { get; set; }
    public string Description { get; set; } = default!;
    public double RecommendedPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreateDate { get; set; }
}
