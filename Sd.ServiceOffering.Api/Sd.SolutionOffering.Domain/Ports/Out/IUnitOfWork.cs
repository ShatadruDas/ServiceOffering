namespace Sd.SolutionOffering.Domain.Ports.Out;

// Unit of Work port: application services commit a complete use case atomically.
public interface IUnitOfWork : IAsyncDisposable
{
    IProductRepository Products { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}