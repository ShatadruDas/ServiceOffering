using Microsoft.EntityFrameworkCore;
using Sd.SolutionOffering.Adapter.Db.Database;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Adapter.Database;

public sealed class SqlCustomerRepository(SqlUnitOfWork unitOfWork) : ICustomerRepository
{
    public async Task<IReadOnlyList<CustomerDomain>> GetAllAsync(CancellationToken cancellationToken)
    {
        var rows = await unitOfWork.Context.Customers
            .AsNoTracking()
            .OrderBy(row => row.LastName)
            .ThenBy(row => row.FirstName)
            .ToListAsync(cancellationToken);
        return rows.Select(Map).ToList();
    }

    public async Task<CustomerDomain?> GetByIdAsync(int customerId, CancellationToken cancellationToken)
    {
        var row = await unitOfWork.Context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.CustomerId == customerId, cancellationToken);
        return row is null ? null : Map(row);
    }

    public async Task<CustomerDomain> AddAsync(CustomerDomain customer, CancellationToken cancellationToken)
    {
        await unitOfWork.EnsureTransactionAsync(cancellationToken);
        var row = new Db.Database.Customer
        {
            Phone = customer.CustomerNumber,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.EmailAddress,
            CreatedAt = customer.CreatedUtc
        };
        unitOfWork.Context.Customers.Add(row);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(row);
    }

    private static CustomerDomain Map(Customer row) => new()
    {
        CustomerId = row.CustomerId,
        CustomerNumber = row.Phone ?? string.Empty,
        FirstName = row.FirstName,
        LastName = row.LastName,
        EmailAddress = row.Email ?? string.Empty,
        IsActive = true,
        CreatedUtc = row.CreatedAt
    };
}