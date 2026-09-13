using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sd.SolutionOffering.Adapter.Db.Database;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Adapter.Database;

// Outbound adapter: this class translates the Unit of Work port to SQL Server transactions.
public sealed class SqlUnitOfWork : IUnitOfWork
{
    private readonly ServiceOfferingDbContext context;
    private IDbContextTransaction? transaction;

    public SqlUnitOfWork(string connectionString)
    {
        var options = new DbContextOptionsBuilder<ServiceOfferingDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        context = new ServiceOfferingDbContext(options);
        context.Database.EnsureCreated();
        Products = new SqlProductRepository(this);
        Customers = new SqlCustomerRepository(this);
        Orders = new SqlOrderRepository(this);
    }

    public IProductRepository Products { get; }
    public ICustomerRepository Customers { get; }
    public IOrderRepository Orders { get; }

    internal ServiceOfferingDbContext Context => context;

    internal async Task EnsureTransactionAsync(CancellationToken cancellationToken) =>
        transaction ??= await context.Database.BeginTransactionAsync(cancellationToken);

    internal Task SaveChangesAsync(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (transaction is null)
        {
            return;
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (transaction is null)
        {
            return;
        }

        await transaction.RollbackAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    private async Task DisposeTransactionAsync()
    {
        if (transaction is not null)
        {
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeTransactionAsync();
        await context.DisposeAsync();
    }
}