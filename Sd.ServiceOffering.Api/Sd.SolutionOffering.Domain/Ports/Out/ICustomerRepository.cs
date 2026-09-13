using Sd.SolutionOffering.Domain.Entities;

namespace Sd.SolutionOffering.Domain.Ports.Out;

// Outbound port: the domain owns this contract; SQL Server is only one possible adapter.
public interface ICustomerRepository
{
    Task<IReadOnlyList<CustomerDomain>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerDomain?> GetByIdAsync(int customerId, CancellationToken cancellationToken);
    Task<CustomerDomain> AddAsync(CustomerDomain customer, CancellationToken cancellationToken);
}