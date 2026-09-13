namespace Sd.SolutionOffering.Domain.Entities;

public sealed class OrderProductDomain
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public required string CurrencyCode { get; init; }
}