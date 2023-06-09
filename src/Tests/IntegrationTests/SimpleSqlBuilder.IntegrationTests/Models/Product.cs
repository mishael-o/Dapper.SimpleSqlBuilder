namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record Product
{
    public int Id { get; set; }
    public Guid GlobalId { get; set; }
    public int? TypeId { get; set; }
    public string? Tag { get; set; }
    public string Description { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
