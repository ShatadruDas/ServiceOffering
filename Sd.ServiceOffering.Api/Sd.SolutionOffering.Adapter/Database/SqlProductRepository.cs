using Microsoft.EntityFrameworkCore;
using Sd.SolutionOffering.Adapter.Db.Database;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.Out;
using ProductDomain = Sd.SolutionOffering.Domain.Entities.Product;

namespace Sd.SolutionOffering.Adapter.Database;

public sealed class SqlProductRepository(SqlUnitOfWork unitOfWork) : IProductRepository
{
    public async Task<IReadOnlyList<ProductDomain>> GetAllAsync(CancellationToken cancellationToken)
    {
        var rows = await unitOfWork.Context.Products
            .AsNoTracking()
            .OrderBy(row => row.Name)
            .ToListAsync(cancellationToken);
        return rows.Select(Map).ToList();
    }

    public async Task<ProductDomain?> GetByIdAsync(int productId, CancellationToken cancellationToken)
    {
        var row = await unitOfWork.Context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.ProductId == productId, cancellationToken);
        return row is null ? null : Map(row);
    }

    public async Task<ProductDomain> AddAsync(ProductDomain product, CancellationToken cancellationToken)
    {
        await unitOfWork.EnsureTransactionAsync(cancellationToken);
        var row = new Db.Database.Product
        {
            Sku = product.ProductCode,
            Name = product.ProductName,
            Description = product.Description,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedUtc
        };
        unitOfWork.Context.Products.Add(row);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(row);
    }

    private static ProductDomain Map(Db.Database.Product row) => new()
    {
        ProductId = row.ProductId,
        ProductCode = row.Sku,
        ProductName = row.Name,
        Description = row.Description,
        IsActive = row.IsActive,
        CreatedUtc = row.CreatedAt
    };
}