using Sd.SolutionOffering.Domain.Entities;

namespace Sd.SolutionOffering.Domain.Ports.In;

// Inbound port: controllers call customer use cases through this stable application boundary.
public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDomain>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerDomain?> GetByIdAsync(int customerId, CancellationToken cancellationToken);
    Task<CustomerDomain> CreateAsync(CustomerDomain customer, CancellationToken cancellationToken);
}