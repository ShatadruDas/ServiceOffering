using Sd.SolutionOffering.Domain.Entities;

namespace Sd.SolutionOffering.Domain.Ports.In;

// Inbound port: order HTTP endpoints use this port without knowing the persistence technology.
public interface IOrderService
{
    Task<OrderDomain?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);
    Task<OrderDomain> CreateAsync(OrderDomain order, CancellationToken cancellationToken);
}