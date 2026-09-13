using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Domain.Services;

// Application service: business use cases depend on abstractions, following Dependency Inversion.
public sealed class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken) =>
        unitOfWork.Products.GetAllAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken) =>
        unitOfWork.Products.GetByIdAsync(productId, cancellationToken);

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(product.ProductCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(product.ProductName);

        var created = await unitOfWork.Products.AddAsync(product, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return created;
    }
}