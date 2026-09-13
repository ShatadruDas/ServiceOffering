using Sd.SolutionOffering.Domain.Entities;

namespace Sd.SolutionOffering.Domain.Ports.Out;

// Outbound port: order persistence includes the order-to-product relationship.
public interface IOrderRepository
{
    Task<OrderDomain?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);
    Task<OrderDomain> AddAsync(OrderDomain order, CancellationToken cancellationToken);
}