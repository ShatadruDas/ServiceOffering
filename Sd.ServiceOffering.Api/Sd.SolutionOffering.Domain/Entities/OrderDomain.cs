namespace Sd.SolutionOffering.Domain.Entities;

public sealed class OrderDomain
{
    public long OrderId { get; init; }
    public required string OrderNumber { get; init; }
    public int CustomerId { get; init; }
    public required string OrderStatus { get; init; }
    public DateTime OrderedUtc { get; init; }
    public IReadOnlyList<OrderProductDomain> Products { get; init; } = [];
}