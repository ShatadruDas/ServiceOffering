using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Domain.Services;

// Application service: customer rules remain independent from the HTTP and database adapters.
public sealed class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
{
    public Task<IReadOnlyList<CustomerDomain>> GetAllAsync(CancellationToken cancellationToken) =>
        unitOfWork.Customers.GetAllAsync(cancellationToken);

    public Task<CustomerDomain?> GetByIdAsync(int customerId, CancellationToken cancellationToken) =>
        unitOfWork.Customers.GetByIdAsync(customerId, cancellationToken);

    public async Task<CustomerDomain> CreateAsync(CustomerDomain customer, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customer.CustomerNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(customer.EmailAddress);

        var created = await unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return created;
    }
}