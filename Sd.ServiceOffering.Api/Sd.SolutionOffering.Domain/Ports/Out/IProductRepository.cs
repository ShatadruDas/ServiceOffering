using Sd.SolutionOffering.Domain.Entities;

namespace Sd.SolutionOffering.Domain.Ports.Out;

// Outbound port: persistence is described by the domain and implemented by an adapter.
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
}