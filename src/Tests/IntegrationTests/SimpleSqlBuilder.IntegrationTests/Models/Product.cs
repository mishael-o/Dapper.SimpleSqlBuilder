namespace Dapper.SimpleSqlBuilder.IntegrationTests.Models;

public record Product
{
    public Guid Id { get; set; }
    public Guid? TypeId { get; set; }
    public string? Tag { get; set; }
    public string Description { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
