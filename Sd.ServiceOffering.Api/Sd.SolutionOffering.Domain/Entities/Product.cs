namespace Sd.SolutionOffering.Domain.Entities;

public sealed class Product
{
    public int ProductId { get; init; }
    public required string ProductCode { get; init; }
    public required string ProductName { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedUtc { get; init; }
}